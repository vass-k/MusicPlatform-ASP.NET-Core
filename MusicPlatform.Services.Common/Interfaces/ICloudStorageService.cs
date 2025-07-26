namespace MusicPlatform.Services.Common.Interfaces
{
    using Microsoft.AspNetCore.Http;

    public interface ICloudStorageService
    {
        Task<string> UploadAudioAsync(IFormFile audioFile);

        Task<string?> UploadImageAsync(IFormFile imageFile);
    }
}
