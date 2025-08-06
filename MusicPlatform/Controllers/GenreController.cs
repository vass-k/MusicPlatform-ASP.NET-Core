namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Genre;

    using static MusicPlatform.GCommon.ApplicationConstants;

    public class GenreController : BaseController
    {
        private readonly IGenreService genreService;
        private readonly ILogger<GenreController> logger;

        public GenreController(IGenreService genreService, ILogger<GenreController> logger)
        {
            this.genreService = genreService;
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<GenreIndexViewModel> allGenres = await this.genreService
                    .GetAllGenresWithTrackCountAsync();

                return View(allGenres);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching the genres index page.");

                TempData[ErrorMessageKey] = "Could not display genres at this time. Please try again later.";

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id, int page = 1)
        {
            if (id == Guid.Empty) return NotFound();
            if (page < 1) page = 1;

            try
            {
                var model = await this.genreService
                    .GetGenreDetailsAsync(id, page, ItemsPerPage);

                if (model == null)
                {
                    logger.LogWarning("A user attempted to access a non-existent genre with ID {GenreId}.", id);

                    return NotFound();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching the details for genre ID {GenreId}.", id);

                TempData[ErrorMessageKey] = "An unexpected error occurred while loading the genre details.";

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
