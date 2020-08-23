using System.Threading.Tasks;
using Chelnak.Blob2S3.Core.IServices;
using Chelnak.Blob2S3.Core.IRepositories;
using Microsoft.Extensions.Logging;
using System.IO;
using System;

namespace Chelnak.Blob2S3.Core.Services
{
    public class FileTransferService : IFileTransferService
    {
        private readonly ILogger<FileTransferService> _logger;
        private readonly IAmazonS3Repository _amazonS3Repository;
        private readonly IBlobStorageRepository _blobStorageRepository;

        public FileTransferService(ILogger<FileTransferService> logger, IAmazonS3Repository amazonS3Repository, IBlobStorageRepository blobStorageRepository)
        {
            _logger = logger;
            _amazonS3Repository = amazonS3Repository;
            _blobStorageRepository = blobStorageRepository;
        }

        public async Task TransferFile(string name)
        {

            if (!await _amazonS3Repository.ValidateBucket())
            {
                throw new Exception("Bucket validation failed. Could not find bucket.");
            }

            var blob = await _blobStorageRepository.GetBlobClient(name);

            if (blob != null)
            {
                using (var mem = new MemoryStream())
                {
                    using (await blob.DownloadToAsync(mem))
                    {
                        await _amazonS3Repository.StreamFileAsync(mem, name);
                    }
                }
            }
        }
    }
}
