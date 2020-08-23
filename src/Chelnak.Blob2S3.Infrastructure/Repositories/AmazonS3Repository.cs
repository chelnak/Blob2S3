using System.Threading.Tasks;
using Chelnak.Blob2S3.Core.IRepositories;
using Chelnak.Blob2S3.Core.Configuration;
using Amazon.S3;
using Amazon.S3.Util;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using System.IO;
using System;
using Microsoft.Extensions.Logging;

namespace Chelnak.Blob2S3.Infrastructure.Repositories
{

    public class AmazonS3Repository : IAmazonS3Repository
    {
        private readonly ILogger<AmazonS3Repository> _logger;
        private readonly AmazonS3Client _client;
        private readonly AmazonS3Settings _settings;
        private readonly TransferUtility _transferUtility;

        public AmazonS3Repository(ILogger<AmazonS3Repository> logger, IOptions<AmazonS3Settings> settings, AmazonS3Client client)
        {
            _logger = logger;
            _settings = settings.Value;
            _client = client;
            _transferUtility = new TransferUtility(_client);
        }

        public async Task<bool> ValidateBucket()
        {
            _logger.LogInformation($"Validating existance of bucket: {_settings.BucketName}.");
            return await AmazonS3Util.DoesS3BucketExistV2Async(_client, _settings.BucketName);
        }

        public async Task StreamFileAsync(MemoryStream stream, string key)
        {
            _logger.LogInformation($"Starting file transfer for {key}.");
            try
            {
                var transferUtilityUploadRequest = new TransferUtilityUploadRequest {
                    InputStream = stream,
                    BucketName = _settings.BucketName,
                    Key = key
                };

                await _transferUtility.UploadAsync(transferUtilityUploadRequest);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while streaming file to Amazon S3: {e}");
            }
        }
    }
}
