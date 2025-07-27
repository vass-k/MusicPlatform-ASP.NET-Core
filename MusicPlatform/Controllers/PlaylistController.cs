namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Playlist;

    using static MusicPlatform.GCommon.ApplicationConstants;
    using static MusicPlatform.Web.ViewModels.ValidationMessages.Playlist;

    public class PlaylistController : BaseController
    {
        private readonly IPlaylistService playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            this.playlistService = playlistService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PlaylistCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                Guid newPlaylistId = await this.playlistService.CreatePlaylistAsync(model, userId);

                TempData[SuccessMessageKey] = $"Playlist '{model.Name}' was created successfully!";

                return RedirectToAction(nameof(Details), new { id = newPlaylistId });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, ServiceCreateError);
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var userId = this.GetUserId();
            var model = await this.playlistService.GetPlaylistDetailsAsync(id, userId);

            if (model == null) return NotFound();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            var model = await this.playlistService
                .GetPlaylistForEditAsync(id, userId);

            if (model == null)
            {
                TempData[ErrorMessageKey] = "Playlist not found or you are not authorized to edit it.";

                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlaylistEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                bool success = await this.playlistService
                    .UpdatePlaylistAsync(model, userId);

                if (!success)
                {
                    TempData[ErrorMessageKey] = "Playlist not found or failed to update.";

                    return NotFound();
                }

                TempData[SuccessMessageKey] = $"Playlist '{model.Name}' was updated successfully!";

                return RedirectToAction(nameof(Details), new { id = model.PublicId });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, ServiceEditError);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                var model = await this.playlistService
                    .GetPlaylistForDeleteAsync(id, userId);
                if (model == null)
                {
                    TempData[ErrorMessageKey] = "Playlist not found or you are not authorized to delete it.";

                    return RedirectToAction(nameof(Index), "Home");
                }

                return View(model);
            }
            catch (Exception)
            {
                // Log error
                return RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(PlaylistDeleteViewModel model)
        {
            var userId = this.GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                bool success = await this.playlistService
                    .DeletePlaylistAsync(model.PublicId, userId);
                if (!success)
                {
                    TempData[ErrorMessageKey] = "Playlist could not be deleted.";

                    return RedirectToAction(nameof(Index), "Home");
                }

                TempData[SuccessMessageKey] = $"Playlist '{model.Name}' was deleted successfully.";

                return RedirectToAction(nameof(Index), "Profile", new { username = this.User.Identity!.Name, tab = "Playlists" });
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "An unexpected error occurred while deleting the playlist.";

                return RedirectToAction(nameof(Index), "Home");
            }
        }

    }
}
