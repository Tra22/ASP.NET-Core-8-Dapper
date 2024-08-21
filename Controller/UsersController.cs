using Microsoft.AspNetCore.Mvc;
using DapperWebAPIProject.Service;
using DapperWebAPIProject.Dto.Request;
using DapperWebAPIProject.Dto.Response;

namespace DapperWebAPIProject.Controller;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] PaginationParamsRequestDto paginationParamsRequestDto)
    {
        var users = await _userService.GetUsers(paginationParamsRequestDto);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userService.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequestDto createUserRequestDto)
    {
        var userId = await _userService.InsertUser(createUserRequestDto);
        return CreatedAtAction(nameof(GetUser), new { id = userId }, createUserRequestDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserRequestDto updateUserRequestDto)
    {
        var user = await _userService.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }
        if (id != user.Id)
        {
            return BadRequest();
        }

        await _userService.UpdateUser(id, updateUserRequestDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUser(id);
        return NoContent();
    }
    [HttpPost("upload")]
    public async Task<ActionResult<List<UserResponseDto>>> UploadInsertAsync(IFormFile file)
    {
        return Ok(await _userService.UploadInsertAsync(file));   
    }
    [HttpPost("upload/merge")]
    public async Task<ActionResult<List<UserResponseDto>>> UploadMergeAsync(IFormFile file)
    {
        return Ok(await _userService.UploadMergeAsync(file));   
    }
}