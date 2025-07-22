namespace MusicPlatform.Web.ViewModels.Playlist
{
    public class PlaylistSelectionViewModel
    {
        public Guid PlaylistPublicId { get; set; }

        public string PlaylistName { get; set; } = null!;

        public bool IsTrackAlreadyInPlaylist { get; set; }
    }
}
