namespace MusicPlatform.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
