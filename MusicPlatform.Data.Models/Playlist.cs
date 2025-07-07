namespace MusicPlatform.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.EntityConstants.Playlist;

    [Comment("This entity represents a playlist of tracks created by user.")]
    public class Playlist
    {
        [Key]
        [Comment("The internal integer id that's primary key for the playlist.")]
        public int Id { get; set; }

        [Required]
        [Comment("The public unique identifier for use in URLs for the playlist.")]
        public Guid PublicId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(NameMaxLength)]
        [Comment("The name of the playlist.")]
        public string Name { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        [Comment("An optional description for the playlist.")]
        public string? Description { get; set; }

        [Comment("Indicates if the playlist is visible to other users.")]
        public bool IsPublic { get; set; } = true;

        [Required]
        [ForeignKey(nameof(Creator))]
        [Comment("The foreign key of the user who created the playlist.")]
        public string CreatorId { get; set; } = null!;

        public virtual AppUser Creator { get; set; } = null!;

        [Comment("Flag that shows if the playlist is soft deleted.")]
        public bool IsDeleted { get; set; } = false;

        [Comment("Timestamp when the playlist was soft deleted.")]
        public DateTime? DeletedOn { get; set; }

        [Comment("The join entity for the many-to-many relationship between Playlist and Track.")]
        public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; }
            = new HashSet<PlaylistTrack>();
    }
}