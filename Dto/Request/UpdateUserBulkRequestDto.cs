namespace DapperWebAPIProject.Dto.Request;
public class UpdateUserBulkRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}