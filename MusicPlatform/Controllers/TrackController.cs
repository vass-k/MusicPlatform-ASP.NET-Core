namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels;
    using MusicPlatform.Web.ViewModels.Track;

    using static GCommon.ApplicationConstants;

    public class TrackController : BaseController
    {
        private readonly ITrackService trackService;
        private const int ItemsPerPage = ItemsPerPageConstant;

        public TrackController(ITrackService trackService)
        {
            this.trackService = trackService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            if (page < 1) page = 1;

            try
            {
                var pagedResult = await this.trackService
                    .GetAllTracksForIndexAsync(page, ItemsPerPage);

                return this.View(pagedResult);
            }
            catch (Exception)
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return this.RedirectToAction(nameof(Index));
            }

            try
            {
                TrackDetailsViewModel? trackDetails = await this.trackService.GetTrackDetailsAsync(id);

                if (trackDetails == null)
                {
                    // TODO: A proper 404 Not Found response.
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(trackDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = await this.trackService.GetTrackAddViewModelAsync();

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(FileValidationConstants.MaxRequestBodySize)]
        public async Task<IActionResult> Add(TrackAddViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Genres = await this.trackService.GetGenresForSelectAsync();
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                var model = await this.trackService.GetTrackForEditAsync(id, userId);
                if (model == null)
                {
                    return NotFound();
                }

                return View(model);
            }
            catch
            {
                // Log error
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TrackEditViewModel model)
        {
            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            if (!ModelState.IsValid)
            {
                model.Genres = await this.trackService.GetGenresForSelectAsync();
                return View(model);
            }

            try
            {
                bool success = await this.trackService.UpdateTrackAsync(model, userId);
                if (!success)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Details), new { id = model.PublicId });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, ValidationMessages.Track.ServiceEditError);

                model.Genres = await this.trackService.GetGenresForSelectAsync();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = this.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                TrackDeleteViewModel? model = await this.trackService.GetTrackForDeleteAsync(id, userId);
                if (model == null)
                {
                    return this.RedirectToAction("Index", "Home");
                }

                return this.View(model);
            }
            catch (Exception)
            {
                // Log error
                return this.RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TrackDeleteViewModel model)
        {
            var userId = this.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                bool success = await this.trackService
                    .DeleteTrackAsync(model.PublicId, userId);

                if (!success)
                {
                    return this.RedirectToAction("Index", "Home");
                }

                return this.RedirectToAction("Index", "Profile", new { username = this.User.Identity!.Name });
            }
            catch (Exception)
            {
                // Log error
                return this.RedirectToAction("Index", "Home");
            }
        }
    }
}
