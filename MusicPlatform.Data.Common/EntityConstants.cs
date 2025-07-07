namespace MusicPlatform.Data.Common
{
    public static class EntityConstants
    {
        public static class Track
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 100;

            public const int ArtistNameMinLength = 2;
            public const int ArtistNameMaxLength = 100;
        }

        public static class Genre
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 500;
        }

        public static class Playlist
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 100;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 500;
        }
    }
}
