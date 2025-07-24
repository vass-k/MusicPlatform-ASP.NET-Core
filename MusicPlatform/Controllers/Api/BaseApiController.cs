namespace MusicPlatform.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Web.Infrastructure.Extensions;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected bool IsUserAuthenticated()
        {
            return this.User
                .IsAuthenticated();
        }

        protected string? GetUserId()
        {
            return this.User
                .GetUserId();
        }
    }
}
