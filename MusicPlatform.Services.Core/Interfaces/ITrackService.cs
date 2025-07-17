namespace MusicPlatform.Services.Core.Interfaces
{
    using MusicPlatform.Web.ViewModels;
    using MusicPlatform.Web.ViewModels.Track;

    public interface ITrackService
    {
        Task<PagedResult<TrackIndexViewModel>> GetAllTracksForIndexAsync(int pageNumber, int pageSize);

        Task<TrackAddViewModel> GetTrackAddViewModelAsync();

        Task AddTrackAsync(TrackAddViewModel model, string uploaderId);

        Task<TrackDetailsViewModel?> GetTrackDetailsAsync(Guid publicId);
    }
}
