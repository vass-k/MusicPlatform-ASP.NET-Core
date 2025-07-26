namespace MusicPlatform.Web.ViewModels.Genre
{
    using System.ComponentModel.DataAnnotations;

    public class GenreEditViewModel : GenreManagementAddViewModel
    {
        [Required]
        public Guid PublicId { get; set; }
    }
}
