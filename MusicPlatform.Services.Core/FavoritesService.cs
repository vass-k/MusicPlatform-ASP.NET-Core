namespace MusicPlatform.Services.Core
{
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Interfaces;

    public class FavoritesService : IFavoritesService
    {
        private readonly ITrackRepository trackRepository;
        private readonly IUserFavoriteRepository favoriteRepository;

        public FavoritesService(ITrackRepository trackRepository, IUserFavoriteRepository favoriteRepository)
        {
            this.trackRepository = trackRepository;
            this.favoriteRepository = favoriteRepository;
        }

        public async Task<int> LikeTrackAsync(Guid trackPublicId, string userId)
        {
            var track = await trackRepository
                .FirstOrDefaultAsync(t => t.PublicId == trackPublicId);
            if (track == null)
            {
                throw new InvalidOperationException("Track not found.");
            }

            var alreadyExists = await favoriteRepository
                .FirstOrDefaultAsync(f => f.UserId == userId && f.TrackId == track.Id);
            if (alreadyExists == null)
            {
                UserFavorite newFavorite = new UserFavorite
                {
                    UserId = userId,
                    TrackId = track.Id
                };

                await favoriteRepository
                    .AddAsync(newFavorite);
            }

            return await trackRepository
                .GetAllAsQueryable().Where(t => t.Id == track.Id).Select(t => t.UserFavorites.Count).FirstAsync();
        }

        public async Task<int> UnlikeTrackAsync(Guid trackPublicId, string userId)
        {
            var track = await trackRepository
                .FirstOrDefaultAsync(t => t.PublicId == trackPublicId);
            if (track == null)
            {
                throw new InvalidOperationException("Track not found.");
            }

            var favorite = await favoriteRepository
                .FirstOrDefaultAsync(f => f.UserId == userId && f.TrackId == track.Id);

            if (favorite != null)
            {
                await favoriteRepository
                    .HardDeleteAsync(favorite);
            }

            return await trackRepository
                .GetAllAsQueryable().Where(t => t.Id == track.Id).Select(t => t.UserFavorites.Count).FirstAsync();
        }
    }
}
