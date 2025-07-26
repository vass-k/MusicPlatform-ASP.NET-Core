namespace MusicPlatform.Web.ViewModels.Admin.GenreManagement
{
    using System.ComponentModel.DataAnnotations;

    public class GenreEditViewModel : GenreManagementAddViewModel
    {
        [Required]
        public Guid PublicId { get; set; }
    }
}
