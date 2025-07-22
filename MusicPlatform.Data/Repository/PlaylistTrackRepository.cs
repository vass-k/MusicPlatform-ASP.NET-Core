namespace MusicPlatform.Data.Repository
{
    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;

    public class PlaylistTrackRepository : BaseRepository<PlaylistTrack, object>, IPlaylistTrackRepository
    {
        public PlaylistTrackRepository(MusicPlatformDbContext dbContext) : base(dbContext)
        {

        }
    }
}
