using Amazon.SQS;
using Amazon.SQS.Model;
using api_image_processor_aws.Models;
using Microsoft.Extensions.Options;

namespace api_image_processor_aws.Services;

public class SqsService : ISqsService
{
    private readonly IAmazonSQS _sqs;
    private readonly SqsOptions _options;

    public SqsService(IAmazonSQS sqs, IOptions<SqsOptions> options)
    {
        _sqs = sqs;
        _options = options.Value;
    }
    
    public async Task<SendMessageResponse> SendMessageAsync(object message)
    {
        var body = System.Text.Json.JsonSerializer.Serialize(message);

        var request = new SendMessageRequest
        {
            QueueUrl = _options.QueueUrl,
            MessageBody = body
        };

        return await _sqs.SendMessageAsync(request);
    }
}