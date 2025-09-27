namespace api_image_processor_aws.Models;

public class S3Options
{
    public string BucketName { get; set; } = "";
    public string? BasePath { get; set; } = "";
}