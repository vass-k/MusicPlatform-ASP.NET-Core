namespace MusicPlatform.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    public class GenreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
