namespace MusicPlatform.Services.Core.Interfaces
{
    using MusicPlatform.Web.ViewModels.Track;

    public interface ITrackService
    {
        Task<IEnumerable<TrackIndexViewModel>> GetAllTracksForIndexAsync();
    }
}
