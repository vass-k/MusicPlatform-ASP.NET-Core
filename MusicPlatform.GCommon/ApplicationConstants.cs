namespace MusicPlatform.GCommon
{
    public static class ApplicationConstants
    {
        public const string IsDeletedPropertyName = "IsDeleted";

        public const string DefaultTrackImageUrl = "/images/default-track-image.png";

        public const int ItemsPerPageConstant = 10;

        public static class FileValidationConstants
        {
            public const int MaxRequestBodySize = 50 * 1024 * 1024; // 50 MB for request body
            public const int MaxAudioFileSize = 50 * 1024 * 1024; // 50 MB for audio
            public const int MaxImageFileSize = 5 * 1024 * 1024; // 5 MB for images

            public static readonly string[] AllowedAudioExtensions = { ".mp3", ".wav", ".flac" };
            public static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png" };
        }
    }
}
