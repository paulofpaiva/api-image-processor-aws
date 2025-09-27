using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Extensions.NETCore.Setup;
using api_image_processor_aws.Middleware;
using api_image_processor_aws.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddAntiforgery(options => options.SuppressXFrameOptionsHeader = true);

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

builder.Services.Configure<S3Options>(builder.Configuration.GetSection("S3"));

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/upload", async (
    [FromForm] IFormFile file,
    IAmazonS3 s3,
    IOptions<S3Options> s3Options
) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("No file sent.");
    }
    
    var bucket = s3Options.Value.BucketName;
    var basePath = s3Options.Value.BasePath ?? "uploads";
    var key = $"{basePath}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
    
    using var stream = file.OpenReadStream();
    var putRequest = new PutObjectRequest
    {
        BucketName = bucket,
        Key = key,
        InputStream = stream,
        ContentType = file.ContentType
    };
    
    await s3.PutObjectAsync(putRequest);
    
    return Results.Ok(ApiResponse<object>.Success(new { Key = key }, "Upload concluído."));
})
.DisableAntiforgery();

app.MapGet("/download/{key}", async (
    [FromQuery] string key,
    IAmazonS3 s3,
    IOptions<S3Options> s3Options
) =>
{
    var bucket = s3Options.Value.BucketName;

    var response = await s3.GetObjectAsync(bucket, key);

    return Results.Ok(ApiResponse<object>.Success(new
    {
        Key = key,
        ContentType = response.Headers.ContentType,
        Size = response.ContentLength
    }));
})
.DisableAntiforgery();

app.MapGet("/list", async (
    IAmazonS3 s3,
    IOptions<S3Options> s3Options
) =>
{
    var bucket = s3Options.Value.BucketName;

    var listRequest = new ListObjectsV2Request
    {
        BucketName = bucket,
        Prefix = s3Options.Value.BasePath ?? "uploads"
    };

    var result = await s3.ListObjectsV2Async(listRequest);

    var files = result.S3Objects.Select(o => new
    {
        Key = o.Key,
        Size = o.Size,
        LastModified = o.LastModified
    });

    return Results.Ok(ApiResponse<object>.Success(files));
});


app.Run();