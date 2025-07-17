namespace MusicPlatform.Web.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public string Username { get; set; } = null!;

        public bool IsCurrentUserProfile { get; set; }

        public string ActiveTab { get; set; } = "Tracks";

        public PagedResult<ProfileTrackViewModel>? UploadedTracks { get; set; }

        public IEnumerable<ProfilePlaylistViewModel>? Playlists { get; set; }
    }
}
