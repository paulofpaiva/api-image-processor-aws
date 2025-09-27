using api_image_processor_aws.DTOs;
using api_image_processor_aws.Services;

namespace api_image_processor_aws.Endpoints;

public static class ListEndpoints
{
    public static void MapListEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/list", async (IS3Service s3Service) =>
            {
                var files = await s3Service.ListAsync();
                return Results.Ok(ApiResponse<object>.Success(files));
            }
        )
        .DisableAntiforgery();
    }
}