using System.Threading.Tasks;

namespace Chelnak.Blob2S3.Core.IServices
{
    public interface IFileTransferService
    {
        Task TransferFile(string name);
    }
}
