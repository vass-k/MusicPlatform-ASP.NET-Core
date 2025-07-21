namespace MusicPlatform.Services.Core.Interfaces
{
    using MusicPlatform.Web.ViewModels.Comment;

    public interface ICommentService
    {
        Task<CommentViewModel> CreateCommentAsync(AddCommentViewModel model, string userId);

        Task<bool> DeleteCommentAsync(int commentId, string currentUserId);
    }
}
