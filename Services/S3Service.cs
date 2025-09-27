using Amazon.S3;
using Amazon.S3.Model;
using api_image_processor_aws.Models;
using Microsoft.Extensions.Options;

namespace api_image_processor_aws.Services;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3;
    private readonly S3Options _options;

    public S3Service(IAmazonS3 s3, IOptions<S3Options> options)
    {
        _s3 = s3;
        _options = options.Value;
    }
    
    public async Task<string> UploadAsync(IFormFile file)
    {
        var bucket = _options.BucketName;
        var basePath = _options.BasePath ?? "uploads";
        var key = $"{basePath}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        using var stream = file.OpenReadStream();
        await _s3.PutObjectAsync(new PutObjectRequest
        {
            BucketName = bucket,
            Key = key,
            InputStream = stream,
            ContentType = file.ContentType
        });

        return key;
    }
    
    public async Task<object> DownloadAsync(string key)
    {
        var response = await _s3.GetObjectAsync(_options.BucketName, key);
        return new
        {
            Key = key,
            ContentType = response.Headers.ContentType,
            Size = response.ContentLength
        };
    }
    
    public async Task<IEnumerable<object>> ListAsync()
    {
        var result = await _s3.ListObjectsV2Async(new ListObjectsV2Request
        {
            BucketName = _options.BucketName,
            Prefix = _options.BasePath ?? "uploads"
        });

        return result.S3Objects.Select(o => new
        {
            Key = o.Key,
            Size = o.Size,
            LastModified = o.LastModified
        });
    }
}