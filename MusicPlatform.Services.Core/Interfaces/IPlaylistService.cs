namespace MusicPlatform.Services.Core.Interfaces
{
    using MusicPlatform.Web.ViewModels.Playlist;

    public interface IPlaylistService
    {
        Task<Guid> CreatePlaylistAsync(PlaylistCreateViewModel model, string userId);

        Task<PlaylistDetailsViewModel?> GetPlaylistDetailsAsync(Guid playlistId, string? currentUserId);
    }
}
