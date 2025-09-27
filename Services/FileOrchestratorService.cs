using api_image_processor_aws.Models;
using Microsoft.Extensions.Options;

namespace api_image_processor_aws.Services;

public interface IFileOrchestratorService
{
    Task<(string Key, string SqsMessageId)> UploadAndNotifyAsync(IFormFile file);
}

public class FileOrchestratorService : IFileOrchestratorService
{
    private readonly IS3Service _s3Service;
    private readonly ISqsService _sqsService;
    private readonly IOptions<S3Options> _s3Options;

    public FileOrchestratorService(
        IS3Service s3Service,
        ISqsService sqsService,
        IOptions<S3Options> s3Options)
    {
        _s3Service = s3Service;
        _sqsService = sqsService;
        _s3Options = s3Options;
    }

    public async Task<(string Key, string SqsMessageId)> UploadAndNotifyAsync(IFormFile file)
    {
        var key = await _s3Service.UploadAsync(file);

        try
        {
            var message = new
            {
                Bucket = _s3Options.Value.BucketName,
                Key = key,
                ContentType = file.ContentType,
                UploadedAt = DateTime.UtcNow
            };

            var response = await _sqsService.SendMessageAsync(message);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Failed to send message to SQS.");

            return (key, response.MessageId);
        }
        catch
        {
            await _s3Service.DeleteAsync(key);
            throw;
        }
    }
}
