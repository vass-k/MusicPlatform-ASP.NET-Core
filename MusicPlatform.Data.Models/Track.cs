namespace MusicPlatform.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.EntityConstants.Track;

    [Comment("This entity represents a single music track in the system.")]
    public class Track
    {
        [Key]
        [Comment("The internal integer id that's primary key for the track.")]
        public int Id { get; set; }

        [Required]
        [Comment("The public unique identifier for use in URLs for the track.")]
        public Guid PublicId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(TitleMaxLength)]
        [Comment("The title of the music track.")]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(ArtistNameMaxLength)]
        [Comment("The name of the artist.")]
        public string ArtistName { get; set; } = null!;

        [Required]
        [Comment("The URL of the audio file stored in a cloud service.")]
        public string AudioUrl { get; set; } = null!;

        [Comment("The URL of the track's cover image, stored in a cloud service.")]
        public string? ImageUrl { get; set; }

        [Comment("The duration of the track in whole seconds.")]
        public int DurationInSeconds { get; set; }

        [Comment("The number of times the track has been played.")]
        public int Plays { get; set; } = 0;

        [Required]
        [ForeignKey(nameof(Uploader))]
        [Comment("The foreign key of the user who uploaded the track.")]
        public string UploaderId { get; set; } = null!;

        public virtual AppUser Uploader { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Genre))]
        [Comment("The foreign key of the track's genre.")]
        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; } = null!;

        [Comment("Flag that shows if track is soft deleted.")]
        public bool IsDeleted { get; set; } = false;

        [Comment("Timestamp when the track was soft deleted.")]
        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<UserFavorite> UserFavorites { get; set; }
            = new HashSet<UserFavorite>();

        public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; }
            = new HashSet<PlaylistTrack>();
    }
}