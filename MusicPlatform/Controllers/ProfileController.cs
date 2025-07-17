namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Interfaces;

    using static GCommon.ApplicationConstants;

    public class ProfileController : BaseController
    {
        private readonly IProfileService profileService;
        private const int ItemsPerPage = ItemsPerPageConstant;

        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string username, int page = 1)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Home");
            }

            if (page < 1) page = 1;

            var currentUserId = this.GetUserId();
            var model = await this.profileService.GetUserProfileAsync(username, page, ItemsPerPage, currentUserId);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
