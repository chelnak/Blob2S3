using System.IO;
using System.Threading.Tasks;

namespace Chelnak.Blob2S3.Core.IRepositories
{
    public interface IAmazonS3Repository
    {
        Task<bool> ValidateBucket();

        Task StreamFileAsync(MemoryStream stream, string key);
    }
}
