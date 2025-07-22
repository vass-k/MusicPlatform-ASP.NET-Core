namespace MusicPlatform.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Playlist;

    [ApiController]
    [Route("api/playlist-tracks")]
    public class PlaylistTracksApiController : BaseController
    {
        private readonly IPlaylistTracksService playlistTracksService;

        public PlaylistTracksApiController(IPlaylistTracksService playlistTracksService)
        {
            this.playlistTracksService = playlistTracksService;
        }

        [HttpGet("user-playlists/{trackPublicId:guid}")]
        public async Task<IActionResult> GetUserPlaylistsForTrack(Guid trackPublicId)
        {
            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            var model = await this.playlistTracksService
                .GetUserPlaylistsAsync(trackPublicId, userId);

            return Ok(model);
        }

        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([FromBody] AddTrackToPlaylistViewModel model)
        {
            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                await this.playlistTracksService
                    .AddTrackToPlaylistAsync(model.TrackPublicId, model.PlaylistPublicId, userId);

                return Ok(new { message = "Track added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove([FromBody] AddTrackToPlaylistViewModel model)
        {
            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                await this.playlistTracksService
                    .RemoveTrackFromPlaylistAsync(model.TrackPublicId, model.PlaylistPublicId, userId);

                return Ok(new { message = "Track removed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
