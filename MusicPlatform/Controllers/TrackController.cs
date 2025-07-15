namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels;
    using MusicPlatform.Web.ViewModels.Track;

    public class TrackController : BaseController
    {
        private readonly ITrackService trackService;

        public TrackController(ITrackService trackService)
        {
            this.trackService = trackService;
        }

        [HttpGet]
        [AllowAnonymous]
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

        [HttpGet]
        public IActionResult Details()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = await this.trackService.GetTrackAddViewModelAsync();

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TrackAddViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var freshModel = await this.trackService.GetTrackAddViewModelAsync();
                model.Genres = freshModel.Genres;
                return this.View(model);
            }

            var userId = this.GetUserId();
            if (userId == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            try
            {
                await this.trackService.AddTrackAsync(model, userId);

                // TODO: Add a TempData success message
                return this.RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);

                var freshModel = await this.trackService.GetTrackAddViewModelAsync();
                model.Genres = freshModel.Genres;
                return this.View(model);
            }
            catch (Exception)
            {
                // That's for unexpected errors (e.g., Cloudinary is down).
                this.ModelState.AddModelError(string.Empty, ValidationMessages.Track.ServiceCreateError);

                var freshModel = await this.trackService.GetTrackAddViewModelAsync();
                model.Genres = freshModel.Genres;
                return this.View(model);
            }
        }
    }
}
