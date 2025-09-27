using Amazon.SQS;
using Amazon.SQS.Model;
using api_image_processor_aws.Models;
using Microsoft.Extensions.Options;

namespace api_image_processor_aws.Services;

public class SqsService : ISqsService
{
    private readonly IAmazonSQS _sqs;
    private readonly SqsOptions _options;
    private readonly S3Options _s3Options;

    public SqsService(IAmazonSQS sqs, IOptions<SqsOptions> options,  IOptions<S3Options> s3Options)
    {
        _sqs = sqs;
        _options = options.Value;
        _s3Options = s3Options.Value;
    }
    
    public async Task<SendMessageResponse> SendMessageAsync(object message)
    {
        var body = System.Text.Json.JsonSerializer.Serialize(message);

        var request = new SendMessageRequest
        {
            QueueUrl = _options.QueueUrl,
            MessageBody = body,
            MessageGroupId = _s3Options.BasePath ?? "default",
            MessageDeduplicationId = Guid.NewGuid().ToString()
        };

        return await _sqs.SendMessageAsync(request);
    }
}