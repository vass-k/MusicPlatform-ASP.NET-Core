namespace MusicPlatform.Data.Repository
{
    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;

    public class UserRepository : BaseRepository<AppUser, string>, IUserRepository
    {
        public UserRepository(MusicPlatformDbContext dbContext) : base(dbContext)
        {

        }
    }
}
