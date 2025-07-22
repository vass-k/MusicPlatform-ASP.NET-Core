namespace MusicPlatform.Services.Core
{
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Playlist;

    public class PlaylistTracksService : IPlaylistTracksService
    {
        private readonly IPlaylistRepository playlistRepository;
        private readonly ITrackRepository trackRepository;
        private readonly IPlaylistTrackRepository playlistTrackRepository;

        public PlaylistTracksService(IPlaylistRepository playlistRepository, ITrackRepository trackRepository, IPlaylistTrackRepository playlistTrackRepository)
        {
            this.playlistRepository = playlistRepository;
            this.trackRepository = trackRepository;
            this.playlistTrackRepository = playlistTrackRepository;
        }

        public async Task<IEnumerable<PlaylistSelectionViewModel>> GetUserPlaylistsAsync(Guid trackPublicId, string userId)
        {
            var track = await this
                .FindTrackByPublicIdAsync(trackPublicId);
            if (track == null) return new List<PlaylistSelectionViewModel>();

            var playlistIdsContainingTrack = await playlistTrackRepository
                .GetAllAsQueryable()
                .Where(pt => pt.TrackId == track.Id)
                .Select(pt => pt.PlaylistId)
                .ToListAsync();

            var userPlaylists = await playlistRepository
                .GetAllAsQueryable()
                .Where(p => p.CreatorId == userId)
                .OrderBy(p => p.Name)
                .Select(p => new PlaylistSelectionViewModel
                {
                    PlaylistPublicId = p.PublicId,
                    PlaylistName = p.Name,
                    IsTrackAlreadyInPlaylist = playlistIdsContainingTrack.Contains(p.Id)
                })
                .ToListAsync();

            return userPlaylists;
        }

        public async Task AddTrackToPlaylistAsync(Guid trackPublicId, Guid playlistPublicId, string userId)
        {
            var playlist = await playlistRepository
                .FirstOrDefaultAsync(p => p.PublicId == playlistPublicId);

            var track = await this
                .FindTrackByPublicIdAsync(trackPublicId);

            if (playlist == null || track == null)
            {
                throw new InvalidOperationException("Playlist or Track not found.");
            }

            if (playlist.CreatorId != userId)
            {
                throw new UnauthorizedAccessException("You are not the owner of this playlist.");
            }

            var alreadyExists = await playlistTrackRepository
                .FirstOrDefaultAsync(pt => pt.PlaylistId == playlist.Id && pt.TrackId == track.Id);

            if (alreadyExists != null)
            {
                return;
            }

            var newEntry = new PlaylistTrack
            {
                PlaylistId = playlist.Id,
                TrackId = track.Id,
                AddedOn = DateTime.UtcNow
            };

            await playlistTrackRepository.AddAsync(newEntry);
        }

        public async Task RemoveTrackFromPlaylistAsync(Guid trackPublicId, Guid playlistPublicId, string userId)
        {
            var playlist = await playlistRepository
                .FirstOrDefaultAsync(p => p.PublicId == playlistPublicId);

            var track = await this
                .FindTrackByPublicIdAsync(trackPublicId);

            if (playlist == null || track == null)
            {
                throw new InvalidOperationException("Playlist or Track not found.");
            }

            if (playlist.CreatorId != userId)
            {
                throw new UnauthorizedAccessException("You are not the owner of this playlist.");
            }

            var entryToRemove = await playlistTrackRepository
                .FirstOrDefaultAsync(pt => pt.PlaylistId == playlist.Id
                                        && pt.TrackId == track.Id);

            if (entryToRemove != null)
            {
                await playlistTrackRepository.HardDeleteAsync(entryToRemove);
            }
        }

        private async Task<Track?> FindTrackByPublicIdAsync(Guid trackPublicId)
        {
            return await trackRepository
                .FirstOrDefaultAsync(t => t.PublicId == trackPublicId);
        }
    }
}
