namespace MusicPlatform.Data.Repository
{
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;

    public class TrackRepository : BaseRepository<Track, int>, ITrackRepository
    {
        public TrackRepository(MusicPlatformDbContext dbContext) : base(dbContext)
        { }

        public async Task<Track?> GetTrackWithDetailsAsync(Guid publicId)
        {
            return await this.DbSet
                .Include(t => t.Uploader)
                .Include(t => t.Genre)
                .Include(t => t.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.PublicId == publicId);
        }

        public async Task<IEnumerable<Track>> GetMostPlayedTracksAsync(int count)
        {
            return await this.DbSet
                .OrderByDescending(t => t.Plays)
                .Take(count)
                .ToListAsync();
        }
    }
}
