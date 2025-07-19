namespace MusicPlatform.Web.ViewModels.Playlist
{
    public class PlaylistDeleteViewModel
    {
        public Guid PublicId { get; set; }

        public string Name { get; set; } = null!;

        public string CreatorUsername { get; set; } = null!;
    }
}
