using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Chelnak.Blob2S3.Functions
{
    public class StartTransfer
    {

        private readonly ILogger<StartTransfer> _logger;

        public StartTransfer(ILogger<StartTransfer> logger)
        {
            _logger = logger;
        }

        [FunctionName("Transfer")]
        public async Task<HttpResponseMessage> Start(
            [HttpTrigger(AuthorizationLevel.Admin, "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter)
        {
            // <object> should be a model
            var eventData = await req.Content.ReadAsAsync<object>();
            string instanceId = await starter.StartNewAsync("Orchestrator", eventData);

            _logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
