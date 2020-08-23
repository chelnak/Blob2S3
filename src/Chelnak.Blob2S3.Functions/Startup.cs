using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Chelnak.Blob2S3.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Chelnak.Blob2S3.Functions;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Chelnak.Blob2S3.Functions
{
    public class Startup : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            builder.Services.AddServices(configuration);
        }
    }
}
