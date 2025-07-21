namespace MusicPlatform.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Comment;

    [ApiController]
    [Route("api/[controller]")]
    public class CommentsApiController : BaseController
    {
        private readonly ICommentService commentService;

        public CommentsApiController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([FromForm] AddCommentViewModel model)
        {
            string? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                CommentViewModel newComment = await commentService.CreateCommentAsync(model, userId);

                return Ok(newComment);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
