using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DapperWebAPIProject.Dto.Request;

public class PaginationParamsRequestDto
{
    private const int MaxPageSize = 100; // Define the maximum page size

    public int PageNumber { get; set; } = 1; // Default to the first page
    private int _pageSize = 10; // Default page size

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; // Limit page size to max
    }
    [BindNever]
    public int Offset => (PageNumber - 1) * PageSize; // Calculate offset based on page number

    // Optionally, you could include validation here
    public void Validate()
    {
        if (PageNumber < 1)
        {
            throw new ArgumentException("Page number must be greater than or equal to 1.");
        }

        if (PageSize < 1)
        {
            throw new ArgumentException("Page size must be greater than or equal to 1.");
        }
    }
}