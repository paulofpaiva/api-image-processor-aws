# API Image Processor AWS

Test API built with **.NET 9** to experiment with **AWS S3** and **SQS** integration.  
Includes basic endpoints to **upload**, **list**, **download** files, and publish events to a queue.

## Features
- `POST /upload` → Upload a file to S3 (stored under `uploads/`) and publish metadata to SQS.
- `GET /list` → List all files in the bucket.
- `GET /download?key=...` → Download a file by key.

## Requirements
- .NET 9 SDK
- AWS S3 bucket + IAM user with permissions (`s3:PutObject`, `s3:GetObject`, `s3:ListBucket`, `s3:DeleteObject`)
- AWS SQS queue + IAM user with permissions (`sqs:SendMessage`)

## Configuration
The S3 bucket name, base path, and SQS queue URL are defined in **`appsettings.json`**:

```json
"S3": {
  "BucketName": "your-bucket-name",
  "BasePath": "uploads"
},
"SQS": {
  "QueueUrl": "https://sqs.<region>.amazonaws.com/<account-id>/<queue-name>.fifo"
}
```

## Run locally
```bash
dotnet restore
dotnet run
