namespace MusicPlatform.Services.Core.Admin.Interfaces
{
    using MusicPlatform.Web.ViewModels.Genre;

    public interface IGenreManagementService
    {
        Task<bool> AddGenreAsync(GenreManagementAddViewModel model);

        Task<GenreEditViewModel?> GetGenreForEditAsync(Guid publicId);

        Task<bool> EditGenreAsync(GenreEditViewModel model);
    }
}
