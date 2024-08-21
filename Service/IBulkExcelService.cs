using FluentValidation;

namespace DapperWebAPIProject.Service;
public interface IBulkExcelService {
    public List<T> ParseExcelFile<T>(string filePath, IValidator<T> validator) where T : new();
}