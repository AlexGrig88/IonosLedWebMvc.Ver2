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
        private readonly SingletonMode _Mode;

        public HomeController(ILogger<HomeController> logger, SingletonMode mode)
        {
            _logger = logger;
            _Mode = mode;
        }

        public IActionResult Index(string homeMode)
        {
            ViewData["HomeMode"] = _Mode.IsDron ? "Дроны" : "Светильники";
            if (homeMode != null) {
                if (homeMode == "DronMode") {
                    _Mode.IsDron = false;
                    ViewData["HomeMode"] = "Светильники";
                }
                else {
                    _Mode.IsDron = true;
                    ViewData["HomeMode"] = "Дроны";
                }
            }
            return View(_Mode);
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
