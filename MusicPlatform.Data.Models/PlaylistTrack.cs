namespace MusicPlatform.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Comment("This entity is a join table for the many-to-many relationship between playlists and tracks.")]
    public class PlaylistTrack
    {
        [Required]
        [ForeignKey(nameof(Playlist))]
        [Comment("The foreign key for the playlist.")]
        public int PlaylistId { get; set; }

        public virtual Playlist Playlist { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Track))]
        [Comment("The foreign key for the track.")]
        public int TrackId { get; set; }

        public virtual Track Track { get; set; } = null!;

        [Comment("The date and time when the track was added to the playlist.")]
        public DateTime AddedOn { get; set; } = DateTime.UtcNow;
    }
}