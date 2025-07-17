namespace MusicPlatform.Services.Core
{
    using Microsoft.EntityFrameworkCore;
    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Playlist;
    using static MusicPlatform.GCommon.ApplicationConstants;

    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository playlistRepository;

        public PlaylistService(IPlaylistRepository playlistRepository)
        {
            this.playlistRepository = playlistRepository;
        }

        public async Task<Guid> CreatePlaylistAsync(PlaylistCreateViewModel model, string userId)
        {
            var newPlaylist = new Playlist()
            {
                PublicId = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                CreatorId = userId
            };

            await this.playlistRepository.AddAsync(newPlaylist);
            return newPlaylist.PublicId;
        }

        public async Task<PlaylistDetailsViewModel?> GetPlaylistDetailsAsync(Guid playlistId, string? currentUserId)
        {
            var playlistDetails = await this.playlistRepository
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(p => p.PublicId == playlistId)
                .Select(p => new PlaylistDetailsViewModel()
                {
                    Name = p.Name,
                    Description = p.Description,
                    CreatorUsername = p.Creator.UserName!,
                    IsOwnedByCurrentUser = (p.CreatorId == currentUserId),
                    TotalDurationSec = p.PlaylistTracks.Sum(pt => pt.Track.DurationInSeconds),
                    Tracks = p.PlaylistTracks
                               .OrderBy(pt => pt.AddedOn)
                               .Select(pt => new PlaylistTrackViewModel
                               {
                                   PublicId = pt.Track.PublicId,
                                   Title = pt.Track.Title,
                                   ArtistName = pt.Track.ArtistName,
                                   ImageUrl = pt.Track.ImageUrl ?? DefaultTrackImageUrl,
                                   DurationInSeconds = pt.Track.DurationInSeconds
                               })
                })
                .FirstOrDefaultAsync();

            return playlistDetails;
        }
    }
}
