using DapperWebAPIProject.Entity;
using DapperWebAPIProject.Data;
using Z.Dapper.Plus;
using System.Text;
using DapperWebAPIProject.Dto.Request;
using DapperWebAPIProject.Dto.Response;

namespace DapperWebAPIProject.Repository;

public class UserRepository : IUserRepository {

    private readonly ILogger<UserRepository> _logger;
    private readonly DataContext _dataContext;
    public UserRepository(ILogger<UserRepository> logger, DataContext dataContext) {
        _logger = logger;
        _dataContext = dataContext;
    }

    public async Task<PaginatedResponseDto<User>> GetUsers(PaginationParamsRequestDto paginationParamsRequestDto) {
        string sql = @"
                SELECT * 
                FROM users
                ORDER BY id
                LIMIT @PageSize
                OFFSET @Offset";
        var content = await _dataContext.QueryAsync<User>(sql, new 
            { 
                PageSize = paginationParamsRequestDto.PageSize, 
                Offset = paginationParamsRequestDto.Offset 
            });

        string countSql = "SELECT COUNT(*) FROM users";
        int totalRecords = await _dataContext.ExecuteScalarAsync<int>(countSql);
        return new PaginatedResponseDto<User>
            {
                PageNumber = paginationParamsRequestDto.PageNumber,
                PageSize = paginationParamsRequestDto.PageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / paginationParamsRequestDto.PageSize),
                Data = content
            };
    }

    public async Task<User> GetUser(int id) {
        return await _dataContext.QuerySingleAsync<User>("SELECT * FROM users WHERE id = @id", new { id = id });
    }

    public async Task<int> InsertUser(User user) {
        return await _dataContext.InsertAsync("INSERT INTO users (name, email) VALUES (@name, @email)", user);
    }

    public async Task<int> UpdateUser(User user) { 
        return await _dataContext.UpdateAsync("UPDATE users SET name = @name, email = @email WHERE id = @id", user);
    }

    public async Task<int> DeleteUser(int id) {
        return await _dataContext.DeleteAsync("DELETE FROM users WHERE id = @id", new { id = id });
    }

    public async Task<IEnumerable<User>> BulkInsertUpload(IEnumerable<User> users)
    {
        using (var connection = _dataContext.CreateConnection())
        {
            var batchId = Guid.NewGuid();
            var userList = users.Select(user => {
                user.BatchId = batchId;
                return user;
            });
            await connection.BulkInsertAsync("users", userList);
            return await _dataContext.QueryAsync<User>("SELECT * FROM users WHERE batch_id = @BatchId", new { BatchId = batchId });
        }
    }
    public async Task<IEnumerable<User>> BulkMergeUpload(IEnumerable<User> users)
    {
        using (var connection = _dataContext.CreateConnection())
        {
            var batchId = Guid.NewGuid();
            var userList = users.Select(user => {
                user.BatchId = batchId;
                return user;
            });
            DapperPlusManager.Entity<User>("users")
                .Identity(x => x.Id, true)
                .IgnoreOnMergeUpdate(x => x.BatchId);
            var sb = new StringBuilder();
            await connection
                .UseBulkOptions(options => 
                { 
                    options.Log = s => sb.AppendLine(s);
                })
                .BulkMergeAsync("users", userList);
            _logger.LogInformation(sb.ToString());
            return await _dataContext.QueryAsync<User>("SELECT * FROM users WHERE batch_id = @BatchId or id = ANY(@ids)", new { BatchId = batchId, ids = users.Select(user => user.Id).ToList() });
        }
    }
}