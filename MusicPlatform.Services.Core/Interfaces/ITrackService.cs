namespace MusicPlatform.Services.Core.Interfaces
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using MusicPlatform.Web.ViewModels;
    using MusicPlatform.Web.ViewModels.Track;

    public interface ITrackService
    {
        Task<PagedResult<TrackIndexViewModel>> GetAllTracksForIndexAsync(int pageNumber, int pageSize);

        Task<TrackIndexPageViewModel> GetTracksForIndexPageAsync(string? searchString, int pageNumber, int pageSize);

        Task<TrackAddViewModel> GetTrackAddViewModelAsync();

        Task AddTrackAsync(TrackAddViewModel model, string uploaderId);

        Task<TrackDetailsViewModel?> GetTrackDetailsAsync(Guid publicId, string? currentUserId);

        Task<TrackEditViewModel?> GetTrackForEditAsync(Guid publicId, string currentUserId);

        Task<bool> UpdateTrackAsync(TrackEditViewModel model, string currentUserId);

        Task<TrackDeleteViewModel?> GetTrackForDeleteAsync(Guid publicId, string currentUserId);

        Task<bool> DeleteTrackAsync(Guid publicId, string currentUserId);

        Task<IEnumerable<SelectListItem>> GetGenresForSelectAsync(int? selectedGenreId = null);
    }
}
