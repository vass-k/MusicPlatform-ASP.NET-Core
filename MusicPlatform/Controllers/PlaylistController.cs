using Microsoft.AspNetCore.Mvc;

namespace MusicPlatform.Web.Controllers
{
    public class PlaylistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
