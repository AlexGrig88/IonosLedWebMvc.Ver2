using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models;
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

        public IActionResult Index()
        {
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
