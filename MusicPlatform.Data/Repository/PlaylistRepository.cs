namespace MusicPlatform.Data.Repository
{
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;

    public class PlaylistRepository : BaseRepository<Playlist, int>, IPlaylistRepository
    {
        public PlaylistRepository(MusicPlatformDbContext dbContext) : base(dbContext)
        { }

        public async Task<Playlist?> GetPlaylistWithTracksAsync(Guid publicId)
        {
            return await this.DbSet
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.Track)
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(p => p.PublicId == publicId);
        }
    }
}
