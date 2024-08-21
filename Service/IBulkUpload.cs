namespace DapperWebAPIProject.Service;
public interface IBulkUpload<T> {
    Task<List<T>> UploadInsertAsync(IFormFile file);
    Task<List<T>> UploadMergeAsync(IFormFile file);
}