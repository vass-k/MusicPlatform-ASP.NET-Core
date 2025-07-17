namespace MusicPlatform.Web.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public string Username { get; set; } = null!;

        public bool IsCurrentUserProfile { get; set; }

        public PagedResult<ProfileTrackViewModel> UploadedTracks { get; set; } = null!;
    }
}
