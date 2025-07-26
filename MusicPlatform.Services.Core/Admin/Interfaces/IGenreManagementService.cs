namespace MusicPlatform.Services.Core.Admin.Interfaces
{
    using MusicPlatform.Web.ViewModels.Admin.GenreManagement;

    public interface IGenreManagementService
    {
        Task<IEnumerable<GenreManagementIndexViewModel>> GetAllGenresForManagementAsync();

        Task<bool> AddGenreAsync(GenreManagementAddViewModel model);

        Task<GenreEditViewModel?> GetGenreForEditAsync(Guid publicId);

        Task<bool> EditGenreAsync(GenreEditViewModel model);

        Task<Tuple<bool, bool>> DeleteOrRestoreGenreAsync(Guid publicId);
    }
}
