namespace api_image_processor_aws.Services;

public class FileValidator : IFileValidator
{
    private readonly long _maxSizeBytes = 5 * 1024 * 1024;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
    private readonly string[] _allowedContentTypes = { "image/jpeg", "image/png", "application/pdf" };
    
    public void Validate(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file sent.");

        if (file.Length > _maxSizeBytes)
            throw new ArgumentException($"File size exceeds {_maxSizeBytes / (1024 * 1024)} MB.");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(ext))
            throw new ArgumentException($"Extension '{ext}' is not allowed.");

        if (!_allowedContentTypes.Contains(file.ContentType))
            throw new ArgumentException($"Content type '{file.ContentType}' is not allowed.");
    }
}