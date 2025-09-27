namespace api_image_processor_aws.Services;

public interface IS3Service
{
    Task<string> UploadAsync(IFormFile file);
    Task<object> DownloadAsync(string key);
    Task<IEnumerable<object>> ListAsync();
    Task DeleteAsync(string key);
}