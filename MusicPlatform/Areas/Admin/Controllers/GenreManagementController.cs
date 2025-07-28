namespace MusicPlatform.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MusicPlatform.Services.Core.Admin.Interfaces;
    using MusicPlatform.Web.ViewModels.Admin.GenreManagement;

    using static MusicPlatform.GCommon.ApplicationConstants;
    using static MusicPlatform.Web.ViewModels.ValidationMessages.Genre;

    public class GenreManagementController : BaseAdminController
    {
        private readonly IGenreManagementService genreManagementService;

        public GenreManagementController(IGenreManagementService genreManagementService)
        {
            this.genreManagementService = genreManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<GenreManagementIndexViewModel> allGenres = await this.genreManagementService
                .GetAllGenresForManagementAsync();

            return View(allGenres);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(GenreManagementAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                bool success = await this.genreManagementService
                    .AddGenreAsync(model);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, GenreAlreadyExists);
                    return View(model);
                }

                TempData[SuccessMessageKey] = $"Genre '{model.Name}' created successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, ServiceCreateError);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                GenreEditViewModel? model = await this.genreManagementService
                    .GetGenreForEditAsync(id);
                if (model == null)
                {
                    TempData[ErrorMessageKey] = "The selected genre could not be found.";

                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "An unexpected error occurred while processing the genre.";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GenreEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                bool success = await this.genreManagementService
                    .EditGenreAsync(model);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, GenreAlreadyExists);
                    return View(model);
                }

                TempData[SuccessMessageKey] = $"Genre '{model.Name}' updated successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                // TODO: Log error and Add TempData error message
                ModelState.AddModelError(string.Empty, ServiceEditError);

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOrRestore(Guid id)
        {
            try
            {
                Tuple<bool, bool> opResult = await this.genreManagementService
                    .DeleteOrRestoreGenreAsync(id);

                bool success = opResult.Item1;
                bool isRestored = opResult.Item2;

                if (!success)
                {
                    TempData[ErrorMessageKey] = "Genre could not be found and was not updated.";
                }
                else
                {
                    string operation = isRestored ? "restored" : "deleted";

                    TempData[SuccessMessageKey] = $"Genre was {operation} successfully!";
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "An unexpected error occurred during the operation.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
