using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Chelnak.Blob2S3.Functions
{
    public class Orchestrator
    {

        private readonly ILogger<Orchestrator> _logger;

        public Orchestrator(ILogger<Orchestrator> logger)
        {
            _logger = logger;
        }

        [FunctionName("Orchestrator")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<string[]>();
            var tasks = new Task<bool>[input.Length];

            _logger.LogInformation($"Starting process ({context.InstanceId}) for {input.Length} files.");

            for (int i = 0; i < input.Length; i++)
            {
                tasks[i] = context.CallActivityAsync<bool>(
                    "FileProcessor",
                    input[i]);
            }
            await Task.WhenAll(tasks);
            _logger.LogInformation($"Process complete ({context.InstanceId}).");
        }
    }
}
