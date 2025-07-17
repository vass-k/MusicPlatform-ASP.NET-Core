namespace MusicPlatform.Web.ViewModels.Playlist
{
    using System.ComponentModel.DataAnnotations;

    using static MusicPlatform.Data.Common.EntityConstants.Playlist;
    using static MusicPlatform.Web.ViewModels.ValidationMessages.Playlist;

    public class PlaylistCreateViewModel
    {
        [Required(ErrorMessage = PlaylistNameRequired)]
        [StringLength(NameMaxLength,
            MinimumLength = NameMinLength,
            ErrorMessage = PlaylistNameLength)]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength,
            ErrorMessage = PlaylistDescriptionLength)]
        public string? Description { get; set; }
    }
}
