# Blob2S3

A durable function for transfering multiple files from an Azure Storage account to Amazon S3.

## local.settings.json
``` json
{
  "IsEncrypted": false,
  "Values": {
    "APPINSIGHTS_INSTRUMENTATIONKEY": "",
    "AzureWebJobsStorage": "",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "AzureStorageSettings:ConnectionString": "",
    "AzureStorageSettings:ContainerName": "blobs",
    "AmazonS3Settings:AccessKey": "",
    "AmazonS3Settings:SecretKey": "",
    "AmazonS3Settings:BucketName": ""
  }
}
```
