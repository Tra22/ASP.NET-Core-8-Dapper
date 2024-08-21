using System.Collections.Immutable;
using System.Reflection;
using DapperWebAPIProject.Attributes;
using DapperWebAPIProject.Util;
using DapperWebAPIProject.Validation.Result;
using FluentValidation;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace DapperWebAPIProject.Service;
public class BulkExcelService : IBulkExcelService {
    private readonly ILogger<BulkExcelService> _logger;
    public BulkExcelService(ILogger<BulkExcelService> logger){
        _logger = logger;
    }
    public List<T> ParseExcelFile<T>(string filePath, IValidator<T> validator) where T : new()
    {
        var list = new List<T>();
        
        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0];
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // assuming the first row is headers
            {
                var entity = new T();
                foreach (var property  in properties)
                {
                    string[] possibleHeaders = GetPossibleHeaders(property);
                    var header = FindHeaderInExcel(worksheet, possibleHeaders);

                    // Find the column index based on the header
                    if (header != null)
                    {
                        var columnIndex = Array.IndexOf(worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column].Select(c => c.Text).ToArray(), header) + 1;
                        if (columnIndex > 0)
                        {
                            var cellValue = worksheet.Cells[row, columnIndex].Text;
                            if (property.CanWrite)
                            {
                                object? value = ConvertToPropertyType(cellValue, property.PropertyType);
                                property.SetValue(entity, value);
                            }
                        }
                    }
                }
                
                // Validate the entity
                var validationResult = validator.Validate(entity);
                if (validationResult.IsValid)
                {
                    list.Add(entity);
                }
                else
                {
                    // Handle validation errors
                    foreach (var error in validationResult.Errors)
                    {
                        _logger.LogError($"Validation failed for row {row}: {error.PropertyName} - {error.ErrorMessage}");
                    }
                    throw new ValidationException(
                        $"Validation failed for row {row}", 
                        validationResult.Errors.Select(s => {
                            return new ValidationFailureList(s, row - 2);
                        }).ToImmutableList()
                    );
                }
            }
        }
        
        return list;
    }
    private object? ConvertToPropertyType(string cellValue, Type targetType)
    {
        if (string.IsNullOrWhiteSpace(cellValue))
        {
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return null; // Handle nullable types
            }else if(NumberUtils.IsNumericType(targetType)){
                return 0;
            }else 
                throw new FormatException($"Cannot convert empty string to type '{targetType.Name}'");
        }

        try
        {
            return Convert.ChangeType(cellValue, targetType);
        }
        catch (InvalidCastException)
        {
            throw new FormatException($"Cannot convert '{cellValue}' to type '{targetType.Name}'");
        }
    }
    private string[] GetPossibleHeaders(PropertyInfo property)
    {
        var jsonPropertyAttribute = property.GetCustomAttribute<JsonPropertyAttribute>();
        var customAttribute = property.GetCustomAttribute<ExcelColumnNameAttribute>();

        // Create an array of possible headers
        var headers = new List<string>();

        if (jsonPropertyAttribute != null && jsonPropertyAttribute.PropertyName != null)
        {
            headers.Add(jsonPropertyAttribute.PropertyName);
        }

        if (customAttribute != null)
        {
            headers.Add(customAttribute.Name);
        }

        // Add fallback to property name
        headers.Add(property.Name);

        return headers.ToArray();
    }

    // Helper method to find the header in Excel
    private string? FindHeaderInExcel(ExcelWorksheet worksheet, string[] possibleHeaders)
    {
        var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column].Select(c => c.Text).ToArray();

        foreach (var header in possibleHeaders)
        {
            if (headerRow.Contains(header))
            {
                return header;
            }
        }

        return null; // No matching header found
    }
}