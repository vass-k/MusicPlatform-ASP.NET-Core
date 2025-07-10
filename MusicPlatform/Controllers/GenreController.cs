using Microsoft.AspNetCore.Mvc;

namespace MusicPlatform.Web.Controllers
{
    public class GenreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
