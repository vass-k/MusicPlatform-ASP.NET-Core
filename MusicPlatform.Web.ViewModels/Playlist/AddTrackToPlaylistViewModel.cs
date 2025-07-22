namespace MusicPlatform.Web.ViewModels.Playlist
{
    using System.ComponentModel.DataAnnotations;

    public class AddTrackToPlaylistViewModel
    {
        [Required]
        public Guid TrackPublicId { get; set; }

        [Required]
        public Guid PlaylistPublicId { get; set; }
    }
}
