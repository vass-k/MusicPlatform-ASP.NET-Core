namespace MusicPlatform.Services.Core.Interfaces
{
    using Microsoft.AspNetCore.Http;

    using System.Threading.Tasks;

    public interface ICloudinaryService
    {
        Task<string> UploadAudioAsync(IFormFile audioFile);
        Task<string?> UploadImageAsync(IFormFile imageFile);
    }
}
