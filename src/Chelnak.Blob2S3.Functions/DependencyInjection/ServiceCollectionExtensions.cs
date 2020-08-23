using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Chelnak.Blob2S3.Core.Configuration;
using Chelnak.Blob2S3.Core.IRepositories;
using Chelnak.Blob2S3.Core.IServices;
using Chelnak.Blob2S3.Core.Services;
using Chelnak.Blob2S3.Infrastructure.Repositories;
using Amazon.S3;
using Amazon;

namespace Chelnak.Blob2S3.DependencyInjection
{

    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging();

            services.Configure<AzureStorageSettings>(configuration.GetSection("AzureStorageSettings"));
            services.AddSingleton(config => config.GetService<IOptions<AzureStorageSettings>>().Value);
            services.AddSingleton(config =>
            {
                var options = config.GetService<IOptions<AzureStorageSettings>>().Value;
                return new BlobServiceClient(options.ConnectionString);
            });

            services.AddSingleton<IBlobStorageRepository, AzureBlobStorageRepository>();

            services.Configure<AmazonS3Settings>(configuration.GetSection("AmazonS3Settings"));
            services.AddSingleton(config => config.GetService<IOptions<AmazonS3Settings>>().Value);
            services.AddSingleton(config =>
            {
                var options = config.GetService<IOptions<AmazonS3Settings>>().Value;
                // Regional endpoint should be from config...
                return new AmazonS3Client(options.AccessKey, options.SecretKey, RegionEndpoint.EUWest2);
            });

            services.AddSingleton<IAmazonS3Repository, AmazonS3Repository>();
            services.AddSingleton<IFileTransferService, FileTransferService>();

            return services;
        }
    }
}
