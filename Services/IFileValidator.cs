namespace api_image_processor_aws.Services;

public interface IFileValidator
{
    void Validate(IFormFile file);
}