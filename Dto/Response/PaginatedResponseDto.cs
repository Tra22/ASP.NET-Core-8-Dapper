namespace DapperWebAPIProject.Dto.Response;

public class PaginatedResponseDto<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public required IEnumerable<T> Data { get; set; }
}