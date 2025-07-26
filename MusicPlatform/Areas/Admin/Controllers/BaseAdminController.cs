namespace MusicPlatform.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Web.Infrastructure.Extensions;

    using static MusicPlatform.GCommon.ApplicationConstants;

    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public abstract class BaseAdminController : Controller
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
