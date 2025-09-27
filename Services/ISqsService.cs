using Amazon.SQS.Model;

namespace api_image_processor_aws.Services;

public interface ISqsService
{
    Task<SendMessageResponse> SendMessageAsync(object message);
}