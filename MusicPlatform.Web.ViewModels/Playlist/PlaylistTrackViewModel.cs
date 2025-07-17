namespace MusicPlatform.Web.ViewModels.Playlist
{
    public class PlaylistTrackViewModel
    {
        public Guid PublicId { get; set; }

        public string Title { get; set; } = null!;

        public string ArtistName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public int DurationInSeconds { get; set; }
    }
}
