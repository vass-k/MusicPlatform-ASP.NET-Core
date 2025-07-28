namespace MusicPlatform.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Admin.Interfaces;
    using MusicPlatform.Web.ViewModels.Admin.UserManagement;

    using static MusicPlatform.GCommon.ApplicationConstants;

    public class UserManagementController : BaseAdminController
    {
        private readonly IUserService userService;

        public UserManagementController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<UserManagementIndexViewModel> allUsers = await this.userService
                .GetUserManagementBoardDataAsync(this.GetUserId()!);

            return View(allUsers);
        }

        [HttpPost]
        public async Task<IActionResult> MakeAdmin(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            try
            {
                bool success = await this.userService
                    .MakeUserAdminAsync(userId);
                if (!success)
                {
                    TempData[ErrorMessageKey] = "User could not be found or failed to be promoted.";
                }
                else
                {
                    TempData[SuccessMessageKey] = "User was successfully promoted to Admin.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "An unexpected error occurred while promoting the user.";

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
