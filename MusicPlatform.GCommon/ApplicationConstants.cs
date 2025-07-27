namespace MusicPlatform.GCommon
{
    public static class ApplicationConstants
    {
        public const string IsDeletedPropertyName = "IsDeleted";

        public const string DefaultTrackImageUrl = "/images/default-track-image.png";

        public const int ItemsPerPageConstant = 12;

        public const string UserRoleName = "User";
        public const string AdminRoleName = "Admin";
        public const string AdminAreaName = "Admin";

        public const string CloudStorageAudioFolder = "music-platform/audio";
        public const string CloudStorageImageFolder = "music-platform/images";

        public const string InfoMessageKey = "info";
        public const string WarningMessageKey = "warning";
        public const string ErrorMessageKey = "error";
        public const string SuccessMessageKey = "success";

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
