using api_image_processor_aws.DTOs;
using api_image_processor_aws.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_image_processor_aws.Endpoints;

public static class UploadEndpoints
{
    public static void MapUploadEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/upload", async (
                [FromForm] IFormFile file, 
                IS3Service s3Service
            ) =>
            {
                if (file == null || file.Length == 0)
                    return Results.Ok(ApiResponse<string>.Fail("No file sent."));

                var key = await s3Service.UploadAsync(file);

                return Results.Ok(ApiResponse<object>.Success(new { Key = key }, "Upload concluído."));
            }
        )
        .DisableAntiforgery();
    }
}