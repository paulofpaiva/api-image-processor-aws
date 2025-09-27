using api_image_processor_aws.DTOs;
using api_image_processor_aws.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_image_processor_aws.Endpoints;

public static class DownloadEndpoints
{
    public static void MapDownloadEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/download/{**key}", async (
                [FromQuery] string key,
                IS3Service s3Service
            ) =>
            {
                var file = await s3Service.DownloadAsync(key);
                return Results.Ok(ApiResponse<object>.Success(file));
            }
        )
        .DisableAntiforgery();
    }
}