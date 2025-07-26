namespace MusicPlatform.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MusicPlatform.Services.Core.Admin.Interfaces;
    using MusicPlatform.Web.ViewModels.Admin.UserManagement;

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
    }
}
