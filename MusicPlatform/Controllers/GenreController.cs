namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class GenreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
