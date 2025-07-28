namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Web.Infrastructure.Extensions;

    using static MusicPlatform.GCommon.ApplicationConstants;

    [Authorize]
    [AutoValidateAntiforgeryToken]
    public abstract class BaseController : Controller
    {
        protected const int ItemsPerPage = ItemsPerPageConstant;

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
