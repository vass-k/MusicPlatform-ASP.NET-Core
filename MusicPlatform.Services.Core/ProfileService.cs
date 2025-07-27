namespace MusicPlatform.Services.Core
{
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels;
    using MusicPlatform.Web.ViewModels.Profile;

    using static GCommon.ApplicationConstants;

    public class ProfileService : IProfileService
    {
        private readonly IUserRepository userRepository;
        private readonly ITrackRepository trackRepository;
        private readonly IPlaylistRepository playlistRepository;
        private readonly IUserFavoriteRepository favoriteRepository;

        public ProfileService(
            IUserRepository userRepository,
            ITrackRepository trackRepository,
            IPlaylistRepository playlistRepository,
            IUserFavoriteRepository favoriteRepository)
        {
            this.userRepository = userRepository;
            this.trackRepository = trackRepository;
            this.playlistRepository = playlistRepository;
            this.favoriteRepository = favoriteRepository;
        }

        public async Task<ProfileViewModel?> GetUserProfileAsync(string username, string activeTab, int pageNumber, int pageSize, string? currentUserId)
        {
            var user = await this.userRepository.FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());
            if (user == null)
            {
                return null;
            }

            var profileViewModel = new ProfileViewModel()
            {
                Username = user.UserName!,
                IsCurrentUserProfile = (user.Id == currentUserId),
                ActiveTab = activeTab
            };

            switch (activeTab)
            {
                case "Tracks":
                default:
                    profileViewModel.UploadedTracks = await this
                        .GetUserUploadedTracksAsync(user, pageNumber, pageSize);
                    break;

                case "Playlists":
                    profileViewModel.Playlists = await this
                        .GetUserPlaylistsAsync(user, currentUserId);
                    break;

                case "Favorites":
                    profileViewModel.FavoriteTracks = await this
                        .GetUserFavoriteTracksAsync(user, pageNumber, pageSize);
                    break;
            }

            return profileViewModel;
        }

        private async Task<PagedResult<ProfileTrackViewModel>> GetUserUploadedTracksAsync(AppUser user, int pageNumber, int pageSize)
        {
            var tracksQuery = this.trackRepository
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(t => t.UploaderId == user.Id);

            var totalTracks = await tracksQuery.CountAsync();

            var pagedTracks = await tracksQuery
                .OrderByDescending(t => t.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new ProfileTrackViewModel()
                {
                    PublicId = t.PublicId,
                    Title = t.Title,
                    ArtistName = t.ArtistName,
                    ImageUrl = t.ImageUrl ?? DefaultTrackImageUrl,
                })
                .ToListAsync();

            var pagedResult = new PagedResult<ProfileTrackViewModel>
            {
                Items = pagedTracks,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(totalTracks / (double)pageSize)
            };

            return pagedResult;
        }

        private async Task<IEnumerable<ProfilePlaylistViewModel>> GetUserPlaylistsAsync(AppUser user, string? currentUserId)
        {
            var UserPlaylists = await this.playlistRepository
               .GetAllAsQueryable()
               .AsNoTracking()
               .Where(p => p.CreatorId == user.Id && (p.IsPublic || p.CreatorId == currentUserId))
               .OrderByDescending(p => p.Id)
               .Select(p => new ProfilePlaylistViewModel
               {
                   PublicId = p.PublicId,
                   Name = p.Name,
                   TrackCount = p.PlaylistTracks.Count()
               })
               .ToListAsync();

            return UserPlaylists;
        }

        private async Task<PagedResult<ProfileTrackViewModel>> GetUserFavoriteTracksAsync(AppUser user, int pageNumber, int pageSize)
        {
            var favoritesQuery = this.favoriteRepository
                                     .GetAllAsQueryable()
                                     .Where(f => f.UserId == user.Id)
                                     .Select(f => f.Track);

            var totalFavorites = await favoritesQuery.CountAsync();
            var pagedFavorites = await favoritesQuery
                .OrderByDescending(t => t.CreatedOn)
                .ThenBy(t => t.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new ProfileTrackViewModel()
                {
                    PublicId = t.PublicId,
                    Title = t.Title,
                    ArtistName = t.ArtistName,
                    ImageUrl = t.ImageUrl ?? DefaultTrackImageUrl,
                })
                .ToListAsync();

            var pagedResult = new PagedResult<ProfileTrackViewModel>
            {
                Items = pagedFavorites,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(totalFavorites / (double)pageSize)
            };

            return pagedResult;
        }
    }
}
