using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Chelnak.Blob2S3.Functions
{
    public static class StartTransfer
    {
        [FunctionName("Transfer")]
        public static async Task<HttpResponseMessage> Start(
            [HttpTrigger(AuthorizationLevel.Admin, "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger logger)
        {
            // <object> should be a model
            var eventData = await req.Content.ReadAsAsync<object>();
            string instanceId = await starter.StartNewAsync("Orchestrator", eventData);

            logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
