namespace MusicPlatform.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Comment("This entity is a join table representing the many-to-many relationship between users and their favorite tracks.")]
    public class UserFavorite
    {
        [Required]
        [ForeignKey(nameof(User))]
        [Comment("The foreign key for the user who favorited the track.")]
        public string UserId { get; set; } = null!;

        public virtual AppUser User { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Track))]
        [Comment("The foreign key for the track that was favorited.")]
        public int TrackId { get; set; }

        public virtual Track Track { get; set; } = null!;
    }
}