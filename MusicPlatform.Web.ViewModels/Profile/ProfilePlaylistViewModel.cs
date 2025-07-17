namespace MusicPlatform.Web.ViewModels.Profile
{
    public class ProfilePlaylistViewModel
    {
        public Guid PublicId { get; set; }

        public string Name { get; set; } = null!;

        public int TrackCount { get; set; }
    }
}
