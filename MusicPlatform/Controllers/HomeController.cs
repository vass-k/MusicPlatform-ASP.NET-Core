namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels;

    using System.Diagnostics;

    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITrackService trackService;

        public HomeController(ILogger<HomeController> logger, ITrackService trackService)
        {
            _logger = logger;
            this.trackService = trackService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = await this.trackService
                .GetTopTracksAsync(10);

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            switch (statusCode)
            {
                case 401:
                case 403:
                    return this.View("UnauthorizedError");
                case 404:
                    return this.View("NotFoundError");
                case 500:
                    return this.View("Error");
                default:
                    return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
