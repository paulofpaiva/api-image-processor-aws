using Amazon.S3;
using api_image_processor_aws.Endpoints;
using api_image_processor_aws.Middleware;
using api_image_processor_aws.Models;
using api_image_processor_aws.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddAntiforgery(options => options.SuppressXFrameOptionsHeader = true);

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

builder.Services.Configure<S3Options>(builder.Configuration.GetSection("S3"));

builder.Services.AddScoped<IS3Service, S3Service>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapUploadEndpoints();
app.MapDownloadEndpoints();
app.MapListEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();