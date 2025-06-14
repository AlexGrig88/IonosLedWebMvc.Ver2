using Microsoft.AspNetCore.Mvc;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class DroneTaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
