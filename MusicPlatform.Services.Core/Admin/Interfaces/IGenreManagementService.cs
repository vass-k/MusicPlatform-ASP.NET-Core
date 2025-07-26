namespace MusicPlatform.Services.Core.Admin.Interfaces
{
    using MusicPlatform.Web.ViewModels.Genre;

    public interface IGenreManagementService
    {
        Task<bool> AddGenreAsync(GenreManagementAddViewModel model);
    }
}
