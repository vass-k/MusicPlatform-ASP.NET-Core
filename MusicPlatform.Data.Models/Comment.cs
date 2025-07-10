namespace MusicPlatform.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.EntityConstants.Comment;

    public class Comment
    {
        [Key]
        [Comment("The internal integer id that's the primary key for the comment.")]
        public int Id { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        [Comment("The text content of the comment.")]
        public string Content { get; set; } = null!;

        [Required]
        [Comment("The date and time when the comment was posted.")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey(nameof(Track))]
        [Comment("The foreign key of the track this comment belongs to.")]
        public int TrackId { get; set; }

        public virtual Track Track { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(User))]
        [Comment("The foreign key of the user who posted this comment.")]
        public string UserId { get; set; } = null!;

        public virtual AppUser User { get; set; } = null!;

        [Comment("Flag that shows if the comment is soft deleted.")]
        public bool IsDeleted { get; set; } = false;

        [Comment("Timestamp when the comment was soft deleted.")]
        public DateTime? DeletedOn { get; set; }
    }
}