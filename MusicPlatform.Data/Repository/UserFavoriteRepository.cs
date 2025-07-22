namespace MusicPlatform.Data.Repository
{
    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;

    public class UserFavoriteRepository : BaseRepository<UserFavorite, object>, IUserFavoriteRepository
    {
        public UserFavoriteRepository(MusicPlatformDbContext dbContext) : base(dbContext)
        {

        }
    }
}
