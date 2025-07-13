namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

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
