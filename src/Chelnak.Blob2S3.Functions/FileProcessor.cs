using System.Threading.Tasks;
using Chelnak.Blob2S3.Core.IServices;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Chelnak.Blob2S3.Functions
{
    public class FileProcessor
    {
        private readonly IFileTransferService _transferService;

        public FileProcessor(IFileTransferService transferService)
        {
            _transferService = transferService;
        }

        [FunctionName("FileProcessor")]
        public async Task ProcessFile([ActivityTrigger] string fileName, ILogger log)
        {
            await _transferService.TransferFile(fileName);
        }
    }
}
