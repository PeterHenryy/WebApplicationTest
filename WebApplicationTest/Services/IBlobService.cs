using System.Reflection.Metadata;

namespace WebApplicationTest.Services
{
    public interface IBlobService
    {
        string GetBlob(string name);
        Task<List<string>> GetAllBlobs();
        Task<bool> UploadBlob(string name, IFormFile file, Blob blob);
        Task<bool> DeleteBlob(string name);
    }
}
