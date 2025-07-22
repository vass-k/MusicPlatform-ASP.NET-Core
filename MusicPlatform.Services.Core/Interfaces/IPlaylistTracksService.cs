namespace MusicPlatform.Services.Core.Interfaces
{
    using MusicPlatform.Web.ViewModels.Playlist;

    public interface IPlaylistTracksService
    {
        Task<IEnumerable<PlaylistSelectionViewModel>> GetUserPlaylistsAsync(Guid trackPublicId, string userId);

        Task AddTrackToPlaylistAsync(Guid trackPublicId, Guid playlistPublicId, string userId);
    }
}
