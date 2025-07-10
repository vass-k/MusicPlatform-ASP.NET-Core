using Microsoft.AspNetCore.Mvc;

namespace MusicPlatform.Web.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
