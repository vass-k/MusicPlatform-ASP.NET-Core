namespace MusicPlatform.Services.Core.Interfaces
{
    using MusicPlatform.Web.ViewModels.Genre;

    public interface IGenreService
    {
        Task<IEnumerable<GenreIndexViewModel>> GetAllGenresWithTrackCountAsync();

        Task<GenreDetailsViewModel?> GetGenreDetailsAsync(Guid publicId, int pageNumber, int pageSize);
    }
}
