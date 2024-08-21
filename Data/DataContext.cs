using System.Data;
using Npgsql;
using Dapper;

namespace DapperWebAPIProject.Data;
public class DataContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DataContext(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
    {
        using (var connection = CreateConnection())
        {
            return await connection.QueryAsync<T>(sql, parameters);
        }
    }
    public async Task<T> QuerySingleAsync<T>(string sql, object? parameters = null) where T : class
    {
        using (var connection = CreateConnection())
        {
            var result = await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
            if (result == null)
            {
                throw new InvalidOperationException("The query returned no results.");
            }
            return result;
        }
    }

    public async Task<int> InsertAsync(string sql, object parameters)
    {
        using (var connection = CreateConnection())
        {
            return await connection.ExecuteAsync(sql, parameters);
        }
    }

    public async Task<int> UpdateAsync(string sql, object parameters)
    {
        using (var connection = CreateConnection())
        {
            return await connection.ExecuteAsync(sql, parameters);
        }
    }

    public async Task<int> DeleteAsync(string sql, object parameters)
    {
        using (var connection = CreateConnection())
        {
            return await connection.ExecuteAsync(sql, parameters);
        }
    }

    public async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        using (var connection = CreateConnection())
        {
            return await connection.ExecuteAsync(sql, parameters);
        }
    }

    public async Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null)
    {
        using (var connection = CreateConnection())
        {
            var result = await connection.ExecuteScalarAsync<T>(sql, parameters);
            if (result == null)
            {
                throw new InvalidOperationException("The query returned no results.");
            }
            return result;
        }
    }
}