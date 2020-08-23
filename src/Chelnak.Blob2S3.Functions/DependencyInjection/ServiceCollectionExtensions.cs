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
using Microsoft.Extensions.Azure;

namespace Chelnak.Blob2S3.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging();

            services.AddOptions<AzureStorageSettings>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("AzureStorageSettings").Bind(settings);
                });

            services.AddOptions<AmazonS3Settings>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("AmazonS3Settings").Bind(settings);
                });

            services.AddAzureClients(builder =>
            {
                var _settings = configuration.GetSection("AzureStorageSettings").Get<AzureStorageSettings>();
                builder.AddBlobServiceClient(_settings.ConnectionString);
            });

            services.AddSingleton(config =>
            {
                var options = config.GetService<IOptions<AmazonS3Settings>>().Value;
                return new AmazonS3Client(options.AccessKey, options.SecretKey, RegionEndpoint.EUWest2);
            });

            services.AddSingleton<IBlobStorageRepository, AzureBlobStorageRepository>();
            services.AddSingleton<IAmazonS3Repository, AmazonS3Repository>();
            services.AddSingleton<IFileTransferService, FileTransferService>();

            return services;
        }
    }
}
