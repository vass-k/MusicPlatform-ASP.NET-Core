﻿namespace MusicPlatform.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
