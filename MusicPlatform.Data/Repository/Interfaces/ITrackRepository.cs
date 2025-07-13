namespace MusicPlatform.Data.Repository.Interfaces
{
    using MusicPlatform.Data.Models;

    public interface ITrackRepository : IBaseRepository<Track, int>
    {
        Task<Track?> GetTrackWithDetailsAsync(Guid publicId);

        Task<IEnumerable<Track>> GetMostPlayedTracksAsync(int count);
    }
}
