namespace api_image_processor_aws.Models;

public class SqsOptions
{
    public string QueueUrl { get; set; } = string.Empty;
    public string Region { get; set; } = "us-east-2";
}