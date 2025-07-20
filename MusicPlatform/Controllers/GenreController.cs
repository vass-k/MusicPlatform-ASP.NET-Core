namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Genre;

    public class GenreController : BaseController
    {
        private readonly IGenreService genreService;

        public GenreController(IGenreService genreService)
        {
            this.genreService = genreService;
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
            catch (Exception e)
            {
                Console.WriteLine(e);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id, int page = 1)
        {
            if (id == Guid.Empty) return BadRequest();
            if (page < 1) page = 1;

            try
            {
                var model = await this.genreService.GetGenreDetailsAsync(id, page, ItemsPerPage);
                if (model == null)
                {
                    return NotFound();
                }

                return View(model);
            }
            catch (Exception)
            {
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
