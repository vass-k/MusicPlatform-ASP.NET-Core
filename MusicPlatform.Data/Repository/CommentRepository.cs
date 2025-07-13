namespace MusicPlatform.Data.Repository
{
    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;

    public class CommentRepository : BaseRepository<Comment, int>, ICommentRepository
    {
        public CommentRepository(MusicPlatformDbContext dbContext) : base(dbContext)
        {

        }
    }

}
