namespace MusicPlatform.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using MusicPlatform.Services.Core.Interfaces;
    using MusicPlatform.Web.ViewModels.Genre;

    public class GenreController : BaseAdminController
    {
        private readonly IGenreService genreService;

        public GenreController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<GenreIndexViewModel> allGenres = await this.genreService
                .GetAllGenresWithTrackCountAsync();

            return View(allGenres);
        }
    }
}
