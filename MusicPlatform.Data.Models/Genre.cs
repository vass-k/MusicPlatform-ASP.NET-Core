namespace MusicPlatform.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    using System.ComponentModel.DataAnnotations;

    using static Common.EntityConstants.Genre;

    [Comment("This entity represents a category for music tracks.")]
    public class Genre
    {
        [Key]
        [Comment("The internal integer id that's primary key for the genre.")]
        public int Id { get; set; }

        [Required]
        [Comment("The public unique identifier for use in URLs for the genre.")]
        public Guid PublicId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(NameMaxLength)]
        [Comment("The name of the genre (e.g., Rock, Pop, Jazz).")]
        public string Name { get; set; } = null!;

        [Comment("Flag that shows if the genre is soft deleted.")]
        public bool IsDeleted { get; set; } = false;

        [Comment("Timestamp when the genre was soft deleted.")]
        public DateTime? DeletedOn { get; set; }

        [Comment("A collection of tracks belonging to this genre.")]
        public virtual ICollection<Track> Tracks { get; set; }
            = new HashSet<Track>();
    }
}