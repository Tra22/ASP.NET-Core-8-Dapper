using DapperWebAPIProject.Dto.Request;
using DapperWebAPIProject.Dto.Response;
using DapperWebAPIProject.Entity;

namespace DapperWebAPIProject.Repository;

public interface IUserRepository { 
    Task<PaginatedResponseDto<User>> GetUsers(PaginationParamsRequestDto paginationParamsRequestDto);
    Task<User> GetUser(int id);
    Task<int> InsertUser(User user);
    Task<int> UpdateUser(User user);
    Task<int> DeleteUser(int id);
    Task<IEnumerable<User>> BulkInsertUpload(IEnumerable<User> users);
    Task<IEnumerable<User>> BulkMergeUpload(IEnumerable<User> users);
}