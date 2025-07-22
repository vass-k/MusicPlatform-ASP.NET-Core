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
            Track? track = await this.
                FindTrackByPublicIdAsync(trackPublicId);
            if (track == null)
            {
                throw new InvalidOperationException("Track not found.");
            }

            UserFavorite? alreadyExists = await this
                .FindFavoriteAsync(track.Id, userId);
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

            return await this
                .GetLikeCountAsync(track.Id);
        }

        public async Task<bool> UnlikeTrackAsync(Guid trackPublicId, string userId)
        {
            Track? track = await this
                .FindTrackByPublicIdAsync(trackPublicId);

            if (track == null)
            {
                return false;
            }

            UserFavorite? favorite = await this
                .FindFavoriteAsync(track.Id, userId);

            if (favorite != null)
            {
                return await favoriteRepository
                    .HardDeleteAsync(favorite);
            }

            return true;
        }

        private async Task<Track?> FindTrackByPublicIdAsync(Guid trackPublicId)
        {
            return await this.trackRepository
                .FirstOrDefaultAsync(t => t.PublicId == trackPublicId);
        }

        private async Task<UserFavorite?> FindFavoriteAsync(int trackId, string userId)
        {
            return await this.favoriteRepository
                .FirstOrDefaultAsync(f => f.UserId == userId && f.TrackId == trackId);
        }

        private async Task<int> GetLikeCountAsync(int trackId)
        {
            return await this.favoriteRepository
                .GetAllAsQueryable()
                .CountAsync(f => f.TrackId == trackId);
        }
    }
}
