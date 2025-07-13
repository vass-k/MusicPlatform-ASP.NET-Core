namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Track;

    public class TrackController : Controller
    {
        private readonly ITrackService trackService;

        public TrackController(ITrackService trackService)
        {
            this.trackService = trackService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<TrackIndexViewModel> allTracks = await this.trackService
                    .GetAllTracksForIndexAsync();

                return this.View(allTracks);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }
    }
}
