namespace MusicPlatform.Services.Core
{
    using MusicPlatform.Data.Models;
    using MusicPlatform.Data.Repository.Interfaces;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Comment;
    using System.Threading.Tasks;

    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly ITrackRepository trackRepository;

        public CommentService(ICommentRepository commentRepository, ITrackRepository trackRepository)
        {
            this.commentRepository = commentRepository;
            this.trackRepository = trackRepository;
        }

        public async Task<CommentViewModel> CreateCommentAsync(AddCommentViewModel model, string userId)
        {
            var track = await trackRepository.FirstOrDefaultAsync(t => t.PublicId == model.TrackPublicId);
            if (track == null)
            {
                throw new InvalidOperationException("Cannot comment on a track that does not exist.");
            }

            Comment newComment = new Comment()
            {
                Content = model.Content,
                TrackId = track.Id,
                UserId = userId,
                CreatedOn = DateTime.UtcNow
            };

            await this.commentRepository.AddAsync(newComment);

            // In order to prevent second DB call for User.UserName, we will use these placeholders.
            // These will be placeholders until we refetch/reload
            return new CommentViewModel
            {
                AuthorUsername = "You",
                Content = newComment.Content,
                PostedOn = "Just now"
            };
        }
    }
}
