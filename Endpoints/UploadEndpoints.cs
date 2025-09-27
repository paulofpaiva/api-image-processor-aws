using api_image_processor_aws.Models;
using api_image_processor_aws.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api_image_processor_aws.Endpoints;

public static class UploadEndpoints
{
    public static void MapUploadEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/upload", async (
                [FromForm] IFormFile file, 
                [FromServices] IFileValidator validator,
                [FromServices] IFileOrchestratorService orchestrator
            ) =>
            {
                validator.Validate(file);
                
                var (key, messageId) = await orchestrator.UploadAndNotifyAsync(file);

                return Results.Ok(ApiResponse<object>.Success(new
                {
                    Key = key,
                    SqsMessageId = messageId
                }, "Uploaded successfully."));
            }
        )
        .DisableAntiforgery();
    }
}