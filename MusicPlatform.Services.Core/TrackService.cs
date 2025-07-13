namespace MusicPlatform.Services.Core
{
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Track;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TrackService : ITrackService
    {
        private readonly ITrackRepository trackRepository;

        public TrackService(ITrackRepository trackRepository)
        {
            this.trackRepository = trackRepository;
        }

        public async Task<IEnumerable<TrackIndexViewModel>> GetAllTracksForIndexAsync()
        {
            var allTracks = await this.trackRepository
                .GetAllWithFavoritesAsync();

            IEnumerable<TrackIndexViewModel> trackViewModels = allTracks
                .Select(t => new TrackIndexViewModel
                {
                    PublicId = t.PublicId,
                    Title = t.Title,
                    ArtistName = t.ArtistName,
                    Plays = t.Plays,
                    FavoritesCount = t.UserFavorites.Count
                });

            return trackViewModels;
        }
    }
}
