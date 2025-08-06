namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels;
    using MusicPlatform.Web.ViewModels.Track;

    using static MusicPlatform.GCommon.ApplicationConstants;

    public class TrackController : BaseController
    {
        private readonly ITrackService trackService;
        private readonly ILogger<TrackController> logger;

        public TrackController(ITrackService trackService, ILogger<TrackController> logger)
        {
            this.trackService = trackService;
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchString, int page = 1)
        {
            if (page < 1) page = 1;

            try
            {
                var trackModel = await this.trackService
                    .GetTracksForIndexPageAsync(searchString, page, ItemsPerPage);

                return this.View(trackModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching track index page with search '{SearchString}' and page {PageNumber}.", searchString, page);

                TempData[ErrorMessageKey] = "Could not load tracks at this time. Please try again later.";

                return this.RedirectToAction(nameof(Index), "Home");
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
                var currentUserId = this.GetUserId();
                TrackDetailsViewModel? trackDetails = await this.trackService
                    .GetTrackDetailsAsync(id, currentUserId);

                if (trackDetails == null)
                {
                    logger.LogWarning("Attempted to access non-existent track with ID {TrackId}.", id);

                    return NotFound();
                }

                return this.View(trackDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching details for track {TrackId}.", id);

                TempData[ErrorMessageKey] = "An unexpected error occurred while loading the track details.";

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = await this.trackService
                .GetTrackAddViewModelAsync();

            return this.View(model);
        }

        [HttpPost]
        [RequestSizeLimit(FileValidationConstants.MaxRequestBodySize)]
        public async Task<IActionResult> Add(TrackAddViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Genres = await this.trackService
                    .GetGenresForSelectAsync();

                return this.View(model);
            }

            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                await this.trackService
                    .AddTrackAsync(model, userId);

                TempData[SuccessMessageKey] = $"Track '{model.Title}' was successfully uploaded!";

                return this.RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning(ex, "User {UserId} failed to upload a track due to a validation error: {ValidationMessage}", userId, ex.Message);

                this.ModelState.AddModelError(string.Empty, ex.Message);

                var freshModel = await this.trackService
                    .GetTrackAddViewModelAsync();

                model.Genres = freshModel.Genres;
                return this.View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error occurred while user {UserId} was adding a track.", userId);

                // That's for unexpected errors (e.g., Cloudinary is down).
                this.ModelState.AddModelError(string.Empty, ValidationMessages.Track.ServiceCreateError);

                var freshModel = await this.trackService.
                    GetTrackAddViewModelAsync();

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
                var model = await this.trackService
                    .GetTrackForEditAsync(id, userId);

                if (model == null)
                {
                    logger.LogWarning("Unauthorized attempt to edit track {TrackId} by user {UserId}.", id, userId);

                    return NotFound();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching track {TrackId} for edit by user {UserId}.", id, userId);

                TempData[ErrorMessageKey] = "An unexpected error occurred.";

                return RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TrackEditViewModel model)
        {
            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            if (!ModelState.IsValid)
            {
                model.Genres = await this.trackService
                    .GetGenresForSelectAsync();

                return View(model);
            }

            try
            {
                bool success = await this.trackService
                    .UpdateTrackAsync(model, userId);

                if (!success)
                {
                    logger.LogWarning("Failed to update track {TrackId} by user {UserId}. It may not exist or they are not the owner.", model.PublicId, userId);

                    return NotFound();
                }

                TempData[SuccessMessageKey] = $"Track '{model.Title}' was updated successfully!";

                return RedirectToAction(nameof(Details), new { id = model.PublicId });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating track {TrackId} by user {UserId}.", model.PublicId, userId);

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
                TrackDeleteViewModel? model = await this.trackService
                    .GetTrackForDeleteAsync(id, userId);

                if (model == null)
                {
                    logger.LogWarning("Unauthorized attempt to get delete confirmation for track {TrackId} by user {UserId}.", id, userId);

                    return NotFound();
                }

                return this.View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching track {TrackId} for deletion by user {UserId}.", id, userId);

                TempData[ErrorMessageKey] = "An unexpected error occurred.";

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TrackDeleteViewModel model)
        {
            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                bool success = await this.trackService
                    .DeleteTrackAsync(model.PublicId, userId);

                if (!success)
                {
                    logger.LogWarning("Failed to delete track {TrackId} by user {UserId}. It may not exist or they are not the owner.", model.PublicId, userId);

                    TempData[ErrorMessageKey] = "Track could not be deleted.";

                    return this.RedirectToAction(nameof(Index), "Home");
                }

                TempData[SuccessMessageKey] = $"Track '{model.Title}' was deleted successfully.";

                return this.RedirectToAction(nameof(Index), "Profile", new { username = this.User.Identity!.Name });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting track {TrackId} by user {UserId}.", model.PublicId, userId);

                TempData[ErrorMessageKey] = "An unexpected error occurred while deleting the track.";

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
