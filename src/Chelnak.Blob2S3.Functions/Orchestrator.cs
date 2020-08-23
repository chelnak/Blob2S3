using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Chelnak.Blob2S3.Functions
{
    public static class Orchestrator
    {

        [FunctionName("Orchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger logger)
        {
            var input = context.GetInput<string[]>();
            var tasks = new Task<string>[input.Length];

            logger.LogInformation($"Starting process ({context.InstanceId}) for {input.Length} files.");

            for (int i = 0; i < input.Length; i++)
            {
                tasks[i] = context.CallActivityAsync<string>(
                    "FileProcessor",
                    input[i]);
            }
            await Task.WhenAll(tasks);

            logger.LogInformation($"Process complete ({context.InstanceId}).");
        }
    }
}
