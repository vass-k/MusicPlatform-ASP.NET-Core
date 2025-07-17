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

        public ProfileService(IUserRepository userRepository, ITrackRepository trackRepository)
        {
            this.userRepository = userRepository;
            this.trackRepository = trackRepository;
        }

        public async Task<ProfileViewModel?> GetUserProfileAsync(string username, int pageNumber, int pageSize, string? currentUserId)
        {
            var user = await this.userRepository.FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());
            if (user == null)
            {
                return null;
            }

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

            var pagedResult = new PagedResult<ProfileTrackViewModel>
            {
                Items = pagedTracks,
                PageNumber = pageNumber,
                TotalPages = totalPages
            };

            var profileViewModel = new ProfileViewModel()
            {
                Username = user.UserName!,
                IsCurrentUserProfile = (user.Id == currentUserId),
                UploadedTracks = pagedResult
            };

            return profileViewModel;
        }

    }
}
