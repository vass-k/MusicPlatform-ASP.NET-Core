namespace MusicPlatform.Web.ViewModels.Playlist
{
    using System.ComponentModel.DataAnnotations;

    using static MusicPlatform.Data.Common.EntityConstants.Playlist;

    public class PlaylistEditViewModel
    {
        public Guid PublicId { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Display(Name = "Make this playlist public")]
        public bool IsPublic { get; set; }
    }
}
