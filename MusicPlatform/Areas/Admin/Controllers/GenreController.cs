namespace MusicPlatform.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class GenreController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
