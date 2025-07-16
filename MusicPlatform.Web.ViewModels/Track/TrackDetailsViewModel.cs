namespace MusicPlatform.Web.ViewModels.Track
{
    using MusicPlatform.Web.ViewModels.Comment;

    public class TrackDetailsViewModel
    {
        public Guid PublicId { get; set; }

        public string Title { get; set; } = null!;

        public string ArtistName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string AudioUrl { get; set; } = null!;

        public int Plays { get; set; }

        public int FavoritesCount { get; set; }

        public int DurationInSeconds { get; set; }

        public string ReleasedDate { get; set; } = null!;

        public IEnumerable<CommentViewModel> Comments { get; set; }
            = new List<CommentViewModel>();
    }
}
