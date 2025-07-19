namespace MusicPlatform.Services.Core
{
    using Microsoft.EntityFrameworkCore;

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

        public ProfileService(
            IUserRepository userRepository,
            ITrackRepository trackRepository,
            IPlaylistRepository playlistRepository)
        {
            this.userRepository = userRepository;
            this.trackRepository = trackRepository;
            this.playlistRepository = playlistRepository;
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
                case "Playlists":
                    profileViewModel.Playlists = await this.playlistRepository
                        .GetAllAsQueryable()
                        .AsNoTracking()
                        .Where(p => p.CreatorId == user.Id &&
                                    (p.IsPublic || p.CreatorId == currentUserId))
                        .OrderByDescending(p => p.Id)
                        .Select(p => new ProfilePlaylistViewModel()
                        {
                            PublicId = p.PublicId,
                            Name = p.Name,
                            TrackCount = p.PlaylistTracks.Count()
                        })
                        .ToListAsync();
                    break;

                case "Tracks":
                default:
                    var tracksQuery = this.trackRepository
                                          .GetAllAsQueryable()
                                          .Where(t => t.UploaderId == user.Id);

                    var totalTracks = await tracksQuery.CountAsync();
                    var totalPages = (int)Math.Ceiling(totalTracks / (double)pageSize);

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

                    profileViewModel.UploadedTracks = new PagedResult<ProfileTrackViewModel>
                    {
                        Items = pagedTracks,
                        PageNumber = pageNumber,
                        TotalPages = totalPages
                    };
                    break;
            }

            return profileViewModel;
        }
    }
}
