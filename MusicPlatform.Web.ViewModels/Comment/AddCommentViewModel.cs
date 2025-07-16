namespace MusicPlatform.Web.ViewModels.Comment
{
    using System.ComponentModel.DataAnnotations;

    using static MusicPlatform.Data.Common.EntityConstants.Comment;

    public class AddCommentViewModel
    {
        [Required]
        public Guid TrackPublicId { get; set; }

        [Required]
        [StringLength(ContentMaxLength,
            MinimumLength = ContentMinLength,
            ErrorMessage = "Comment must be between {2} and {1} characters.")]
        public string Content { get; set; } = null!;
    }
}
