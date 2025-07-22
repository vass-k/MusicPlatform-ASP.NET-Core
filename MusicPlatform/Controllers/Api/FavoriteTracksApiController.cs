namespace MusicPlatform.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using MusicPlatform.Services.Core.Interfaces;

    [ApiController]
    [Route("api/favorites")]
    public class FavoriteTracksApiController : BaseController
    {
        private readonly IFavoritesService favoritesService;

        public FavoriteTracksApiController(IFavoritesService favoritesService)
        {
            this.favoritesService = favoritesService;
        }

        [HttpPost("like/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like(Guid id)
        {
            var userId = this.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "You must be logged in to like a track." });
            }

            try
            {
                var newLikeCount = await this.favoritesService
                    .LikeTrackAsync(id, userId);

                return Ok(new { newLikeCount });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("unlike/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlike(Guid id)
        {
            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                bool success = await this.favoritesService
                    .UnlikeTrackAsync(id, userId);

                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
