namespace MusicPlatform.Data.Repository
{
    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;

    public class GenreRepository : BaseRepository<Genre, int>, IGenreRepository
    {
        public GenreRepository(MusicPlatformDbContext dbContext) : base(dbContext)
        {

        }
    }

}
