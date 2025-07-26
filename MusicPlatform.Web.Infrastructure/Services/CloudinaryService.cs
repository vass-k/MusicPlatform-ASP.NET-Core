namespace MusicPlatform.Web.Infrastructure.Services
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    using MusicPlatform.Services.Common.Interfaces;
    using MusicPlatform.Web.Infrastructure.Configuration;

    using System.Threading.Tasks;

    using static MusicPlatform.GCommon.ApplicationConstants;

    public class CloudinaryService : ICloudStorageService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret);

            cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadAudioAsync(IFormFile audioFile)
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(audioFile.FileName, audioFile.OpenReadStream()),
                Folder = CloudStorageAudioFolder
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new InvalidOperationException($"Cloudinary audio upload failed: {uploadResult.Error.Message}");
            }

            return uploadResult.SecureUrl.ToString();
        }

        public async Task<string?> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return null;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
                Transformation = new Transformation().Height(400).Width(400).Crop("fill").Gravity("face"),
                Folder = CloudStorageImageFolder
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new InvalidOperationException($"Cloudinary image upload failed: {uploadResult.Error.Message}");
            }

            return uploadResult.SecureUrl.ToString();
        }
    }
}
