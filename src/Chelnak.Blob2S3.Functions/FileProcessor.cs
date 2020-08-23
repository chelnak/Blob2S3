using System.Threading.Tasks;
using Chelnak.Blob2S3.Core.IServices;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Chelnak.Blob2S3.Functions
{
    public class FileProcessor
    {
        private readonly ILogger<FileProcessor> _logger;
        private readonly IFileTransferService _transferService;

        public FileProcessor(ILogger<FileProcessor> logger, IFileTransferService transferService)
        {
            _logger = logger;
            _transferService = transferService;
        }

        [FunctionName("FileProcessor")]
        public async Task<bool> ProcessFile([ActivityTrigger] string fileName)
        {
            await _transferService.TransferFile(fileName);
            return true;
        }
    }
}
