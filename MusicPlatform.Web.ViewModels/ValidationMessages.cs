namespace MusicPlatform.Web.ViewModels
{
    public static class ValidationMessages
    {
        public static class Track
        {
            public const string TitleRequired = "The track title is required.";
            public const string TitleLength = "Title must be between {2} and {1} characters.";

            public const string ArtistNameRequired = "The artist's name is required.";
            public const string ArtistNameLength = "Artist name must be between {2} and {1} characters.";

            public const string GenreRequired = "Please select a genre for the track.";

            public const string AudioFileRequired = "Please upload an audio file for the track.";

            public const string ServiceCreateError =
                "Fatal error occurred while adding your track! Please try again later!";

            public const string ServiceEditError =
                "Fatal error occurred while editing your track! Please try again later!";
        }

        public static class Playlist
        {
            public const string PlaylistNameRequired = "Playlist name is required.";
            public const string PlaylistNameLength = "Playlist name must be between {2} and {1} characters.";
            public const string PlaylistDescriptionLength = "Description must not exceed {1} characters.";

            public const string ServiceCreateError =
                "Fatal error occurred while creating your playlist! Please try again later!";

            public const string ServiceEditError =
                "Fatal error occurred while editing your playlist! Please try again later!";
        }

        public static class Comment
        {
            public const string FatalErrorOccurred =
                "Fatal error occurred while processing your comment! Please try again later!";
        }

        public static class Genre
        {
            public const string GenreNameRequired = "Genre name is required.";
            public const string GenreNameLength = "Genre name must be between {2} and {1} characters.";

            public const string GenreAlreadyExists = "A genre with this name already exists.";

            public const string ServiceCreateError =
                "Fatal error occurred while creating the genre! Please try again later!";

            public const string ServiceEditError =
                "Fatal error occurred while editing the genre! Please try again later!";
        }
    }
}
