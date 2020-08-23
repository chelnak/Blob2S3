using Azure.Storage.Blobs;
using System.Threading.Tasks;

namespace Chelnak.Blob2S3.Core.IRepositories
{
    public interface IBlobStorageRepository
    {
        Task<BlobClient> GetBlobClient(string blobName);
    }
}
