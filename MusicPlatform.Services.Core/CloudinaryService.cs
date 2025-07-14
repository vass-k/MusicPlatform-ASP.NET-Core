namespace MusicPlatform.Services.Core
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    using MusicPlatform.GCommon;
    using MusicPlatform.Services.Core.Interfaces;

    using System.Threading.Tasks;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadAudioAsync(IFormFile audioFile)
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(audioFile.FileName, audioFile.OpenReadStream()),
                Folder = "music-platform/audio"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUrl.ToString();
        }

        public async Task<string?> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return null;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
                Transformation = new Transformation().Height(400).Width(400).Crop("fill").Gravity("face"),
                Folder = "music-platform/images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }
    }
}
