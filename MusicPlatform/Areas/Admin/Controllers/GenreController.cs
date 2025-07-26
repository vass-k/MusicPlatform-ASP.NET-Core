namespace MusicPlatform.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Admin.Interfaces;
    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Genre;

    using static MusicPlatform.Web.ViewModels.ValidationMessages.Genre;

    public class GenreController : BaseAdminController
    {
        private readonly IGenreService genreService;
        private readonly IGenreManagementService genreManagementService;

        public GenreController(IGenreService genreService, IGenreManagementService genreManagementService)
        {
            this.genreService = genreService;
            this.genreManagementService = genreManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<GenreIndexViewModel> allGenres = await this.genreService
                .GetAllGenresWithTrackCountAsync();

            return View(allGenres);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, ServiceCreateError);
                return View(model);
            }
        }
    }
}
