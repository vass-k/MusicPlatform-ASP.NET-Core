namespace MusicPlatform.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Track;

    [ApiController]
    public class TrackPlayApiController : BaseApiController
    {
        private readonly ITrackService trackService;

        public TrackPlayApiController(ITrackService trackService)
        {
            this.trackService = trackService;
        }

        [AllowAnonymous]
        [HttpPost("increment-play")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IncrementPlayCount([FromBody] TrackPlayCountViewModel request)
        {
            if (request.TrackId == Guid.Empty)
            {
                return BadRequest();
            }

            await this.trackService
                .IncrementdPlayCountAsync(request.TrackId);

            return NoContent();
        }
    }
}
