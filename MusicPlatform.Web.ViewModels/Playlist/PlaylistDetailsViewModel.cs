namespace MusicPlatform.Web.ViewModels.Playlist
{
    public class PlaylistDetailsViewModel
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string CreatorUsername { get; set; } = null!;

        public int TotalDurationSec { get; set; }

        public bool IsOwnedByCurrentUser { get; set; }

        public IEnumerable<PlaylistTrackViewModel> Tracks { get; set; }
            = new List<PlaylistTrackViewModel>();
    }
}
