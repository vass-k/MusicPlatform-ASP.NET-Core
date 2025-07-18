namespace MusicPlatform.Data.Repository.Interfaces
{
    using MusicPlatform.Data.Models;

    using System;
    using System.Threading.Tasks;

    public interface IPlaylistRepository : IBaseRepository<Playlist, int>
    {
        Task<Playlist?> GetPlaylistWithTracksAsync(Guid publicId);
    }
}
