namespace MusicPlatform.Services.Core.Interfaces
{
    public interface IFavoritesService
    {
        Task<int> LikeTrackAsync(Guid trackPublicId, string userId);

        Task<int> UnlikeTrackAsync(Guid trackPublicId, string userId);
    }
}
