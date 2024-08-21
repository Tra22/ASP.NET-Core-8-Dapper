using DapperWebAPIProject.Dto.Response;
using DapperWebAPIProject.Dto.Request;

namespace DapperWebAPIProject.Service;
public interface IUserService : IBulkUpload<UserResponseDto> {
    Task<PaginatedResponseDto<UserResponseDto>> GetUsers(PaginationParamsRequestDto paginationParamsRequestDto);
    Task<UserResponseDto> GetUser(int id);
    Task<int> InsertUser(CreateUserRequestDto createUserRequestDto);
    Task<int> UpdateUser(int id, UpdateUserRequestDto updateUserRequestDto);
    Task<int> DeleteUser(int id);
}