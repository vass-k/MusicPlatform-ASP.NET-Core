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

        public async Task<PlaylistEditViewModel?> GetPlaylistForEditAsync(Guid publicId, string currentUserId)
        {
            var playlist = await this
                .FindPlaylistByPublicIdAsync(publicId);

            if (playlist == null || playlist.CreatorId != currentUserId)
            {
                return null;
            }

            return new PlaylistEditViewModel
            {
                PublicId = playlist.PublicId,
                Name = playlist.Name,
                Description = playlist.Description,
                IsPublic = playlist.IsPublic
            };
        }

        public async Task<bool> UpdatePlaylistAsync(PlaylistEditViewModel model, string currentUserId)
        {
            var playlist = await this
                .FindPlaylistByPublicIdAsync(model.PublicId);

            if (playlist == null || playlist.CreatorId != currentUserId)
            {
                return false;
            }

            playlist.Name = model.Name;
            playlist.Description = model.Description;
            playlist.IsPublic = model.IsPublic;

            return await this.playlistRepository.UpdateAsync(playlist);
        }

        public async Task<PlaylistDeleteViewModel?> GetPlaylistForDeleteAsync(Guid publicId, string currentUserId)
        {
            var playlist = await this.playlistRepository
                .GetAllAsQueryable()
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(p => p.PublicId == publicId);

            if (playlist == null || playlist.CreatorId != currentUserId)
            {
                return null;
            }

            return new PlaylistDeleteViewModel
            {
                PublicId = playlist.PublicId,
                Name = playlist.Name,
                CreatorUsername = playlist.Creator.UserName!
            };
        }

        public async Task<bool> DeletePlaylistAsync(Guid publicId, string currentUserId)
        {
            var playlist = await this
                .FindPlaylistByPublicIdAsync(publicId);

            if (playlist == null || playlist.CreatorId != currentUserId)
            {
                return false;
            }

            return await this.playlistRepository.DeleteAsync(playlist);
        }

        private async Task<Playlist?> FindPlaylistByPublicIdAsync(Guid publicId)
        {
            return await this.playlistRepository
                .FirstOrDefaultAsync(p => p.PublicId == publicId);
        }
    }
}
