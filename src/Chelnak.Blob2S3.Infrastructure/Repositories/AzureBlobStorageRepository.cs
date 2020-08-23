using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Chelnak.Blob2S3.Core.IRepositories;
using Chelnak.Blob2S3.Core.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Chelnak.Blob2S3.Infrastructure.Repositories
{
    public class AzureBlobStorageRepository : IBlobStorageRepository
    {
        private readonly ILogger<AzureBlobStorageRepository> _logger;
        private readonly BlobContainerClient _client;
        private readonly AzureStorageSettings _settings;

        public AzureBlobStorageRepository(ILogger<AzureBlobStorageRepository> logger, IOptions<AzureStorageSettings> settings, BlobServiceClient client)
        {
            _logger = logger;
            _settings = settings.Value;
            _client = client.GetBlobContainerClient(_settings.ContainerName);
        }

        public async Task<BlobClient> GetBlobClient(string blobName)
        {
            _logger.LogInformation($"Retrieving blob client for: {blobName}.");
            await _client.CreateIfNotExistsAsync();

            var blob = _client.GetBlobClient(blobName);
            if (! await blob.ExistsAsync())
            {
                _logger.LogError($"A blob with name {blobName} does not exist in the sources storage account.");
            }
            return blob;
        }
    }
}
