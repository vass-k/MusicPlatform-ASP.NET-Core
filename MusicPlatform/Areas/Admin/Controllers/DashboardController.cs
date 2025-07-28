namespace MusicPlatform.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Admin.Interfaces;

    public class DashboardController : BaseAdminController
    {
        private readonly IDashboardService dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.dashboardService
                .GetDashboardDataAsync();

            return View(model);
        }
    }
}
