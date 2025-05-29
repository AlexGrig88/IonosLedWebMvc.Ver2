using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models;
using IonosLedWebMvc.Ver2.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string appMode)
        {
            if (appMode == null && !HttpContext.Session.Keys.Contains("AppMode")) {
                appMode = "LampMode";
            }
            appMode = appMode == null ? HttpContext.Session.GetString("AppMode") : appMode;
            ViewData["AppMode"] = appMode;
            HttpContext.Session.SetString("CurrentUser", "User");
            HttpContext.Session.SetString("AppMode", appMode);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
		public IActionResult Forbidden()
		{
			return View();
		}
		

	}
}
