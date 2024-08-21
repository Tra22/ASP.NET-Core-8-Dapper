using DapperWebAPIProject.Repository;
using DapperWebAPIProject.Entity;
using DapperWebAPIProject.Dto.Request;
using DapperWebAPIProject.Dto.Response;
using AutoMapper;
using DapperWebAPIProject.Validation;

namespace DapperWebAPIProject.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IBulkExcelService _bulkExcelService;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IBulkExcelService bulkExcelService, IMapper mapper)
    {
        _userRepository = userRepository;
        _bulkExcelService = bulkExcelService;
        _mapper = mapper;
    }

    public async Task<PaginatedResponseDto<UserResponseDto>> GetUsers(PaginationParamsRequestDto paginationParamsRequestDto)
    {
        var results = await _userRepository.GetUsers(paginationParamsRequestDto);
        return new PaginatedResponseDto<UserResponseDto>{
            PageNumber = results.PageNumber,
            PageSize = results.PageSize,
            TotalRecords = results.TotalRecords,
            TotalPages = results.TotalPages,
            Data = _mapper.Map<UserResponseDto[]>(results.Data)
        };
    }

    public async Task<UserResponseDto> GetUser(int id)
    {
        var result = await _userRepository.GetUser(id);
        return _mapper.Map<UserResponseDto>(result);
    }

    public async Task<int> InsertUser(CreateUserRequestDto createUserRequestDto)
    {
        return await _userRepository.InsertUser(_mapper.Map<User>(createUserRequestDto));
    }

    public async Task<int> UpdateUser(int id, UpdateUserRequestDto updateUserRequestDto)
    {

        User user = _mapper.Map<User>(updateUserRequestDto);
        user.Id = id;
        return await _userRepository.UpdateUser(user);
    }

    public async Task<int> DeleteUser(int id)
    {
        return await _userRepository.DeleteUser(id);
    }

    public async Task<List<UserResponseDto>> UploadInsertAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new Exception("No file uploaded");
        }

        // Generate a temporary file path
        var tempFilePath = Path.GetTempFileName();

        // Save the uploaded file to the temporary path
        using (var stream = new FileStream(tempFilePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var validator = new CreateUserDtoValidator();

        // Parse the Excel file
        var users = _bulkExcelService.ParseExcelFile(tempFilePath, validator);

        // Optionally, remove the temporary file
        System.IO.File.Delete(tempFilePath);

        // Process the list of users as needed (e.g., save to database)

        IEnumerable<User> usersEntity = _mapper.Map<User[]>(users);
        usersEntity = await _userRepository.BulkInsertUpload(usersEntity);
        return _mapper.Map<List<UserResponseDto>>(usersEntity);
    }

    public async Task<List<UserResponseDto>> UploadMergeAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new Exception("No file uploaded");
        }

        // Generate a temporary file path
        var tempFilePath = Path.GetTempFileName();

        // Save the uploaded file to the temporary path
        using (var stream = new FileStream(tempFilePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var validator = new UpdateUserBulkDtoValidator();

        // Parse the Excel file
        var users = _bulkExcelService.ParseExcelFile(tempFilePath, validator);

        // Optionally, remove the temporary file
        System.IO.File.Delete(tempFilePath);

        // Process the list of users as needed (e.g., save to database)

        IEnumerable<User> usersEntity = _mapper.Map<User[]>(users);
        usersEntity = await _userRepository.BulkMergeUpload(usersEntity);
        return _mapper.Map<List<UserResponseDto>>(usersEntity);
    }
}