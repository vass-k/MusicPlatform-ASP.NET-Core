namespace MusicPlatform.Services.Core.Interfaces
{
    using MusicPlatform.Web.ViewModels.Track;

    public interface ITrackService
    {
        Task<IEnumerable<TrackIndexViewModel>> GetAllTracksForIndexAsync();

        Task<TrackAddViewModel> GetTrackAddViewModelAsync();

        Task AddTrackAsync(TrackAddViewModel model, string uploaderId);

        Task<TrackDetailsViewModel?> GetTrackDetailsAsync(Guid publicId);
    }
}
