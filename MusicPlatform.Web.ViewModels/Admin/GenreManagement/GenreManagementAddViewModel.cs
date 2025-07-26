namespace MusicPlatform.Web.ViewModels.Admin.GenreManagement
{
    using System.ComponentModel.DataAnnotations;

    using static MusicPlatform.Data.Common.EntityConstants.Genre;
    using static ValidationMessages.Genre;

    public class GenreManagementAddViewModel
    {
        [Required(ErrorMessage = GenreNameRequired)]
        [StringLength(NameMaxLength,
                        MinimumLength = NameMinLength,
                        ErrorMessage = GenreNameLength)]
        public string Name { get; set; } = null!;
    }
}
