# API Image Processor AWS

Test API built with **.NET 9** to experiment with **AWS S3** integration.  
Includes basic endpoints to **upload**, **list**, and **download** files.


## Features
- `POST /upload` → Upload a file to S3 (stored under `uploads/`).
- `GET /list` → List all files in the bucket.
- `GET /download?key=...` → Download a file by key.

## Requirements
- .NET 9 SDK
- AWS S3 bucket + IAM user with permissions (`s3:PutObject`, `s3:GetObject`, `s3:ListBucket`)

## Configuration
The S3 bucket name and base path are defined in **`appsettings.json`**:

```json
"S3": {
  "BucketName": "your-bucket-name",
  "BasePath": "uploads"
}

## Run locally
```bash
dotnet restore
dotnet run
