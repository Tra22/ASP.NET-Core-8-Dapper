using AutoMapper;
using DapperWebAPIProject.Dto.Request;
using DapperWebAPIProject.Dto.Response;
using DapperWebAPIProject.Entity;

namespace DapperWebAPIProject.Mapper;
public class UserProfileMapper : Profile {
    public UserProfileMapper()
    {
        // Default mapping when property names are same
        CreateMap<User, UserResponseDto>();
        CreateMap<CreateUserRequestDto, User>();     
        CreateMap<UpdateUserRequestDto, User>();
        CreateMap<UpdateUserBulkRequestDto, User>();
    }
}