using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class SalaryController : Controller
    {

        private readonly ApplicationContext _context;

        public SalaryController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var filter = new LampFilterDto() { StartDate = new DateTime(2024, 03, 2), EndDate = new DateTime(2024, 04, 2) };
            ViewBag.AllModels = new List<string>() { filter.ModelName };
            ViewBag.AllEmployees = new List<string>() { filter.EmployeeName };
            return View(filter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RunFilter([Bind("StartDate,EndDate,EmployeeName,ModelName")] LampFilterDto lampFilterDto)
        {
            if (lampFilterDto.StartDate > lampFilterDto.EndDate) {
                ViewBag.AllModels = new List<string>() { lampFilterDto.ModelName };
                ViewBag.AllEmployees = new List<string>() { lampFilterDto.EmployeeName };
                return View("Index", lampFilterDto);
            }

            if (lampFilterDto.EndDate - lampFilterDto.StartDate > new TimeSpan(92, 0, 0, 0)) {
                ViewBag.AllModels = new List<string>() { lampFilterDto.ModelName };
                ViewBag.AllEmployees = new List<string>() { lampFilterDto.EmployeeName };
                return View("Index", lampFilterDto); ;
            }

            ViewBag.StartDate = lampFilterDto.StartDate;
            ViewBag.EndDate = lampFilterDto.EndDate;
            ViewBag.EmployeeName = lampFilterDto.EmployeeName;
            ViewBag.ModelName = lampFilterDto.ModelName;

            List<LedLamp> products = await _context.LedLamps
                .Include(l => l.AssemblingUser)
                .Include(l => l.CheckingPackagingUser)
                .Include(l => l.CutUser)
                .Include(l => l.DrillUser)
                .Include(l => l.LabelPrintUser)
                .Include(l => l.Model)
                .Include(l => l.MountingUser)
                .Include(l => l.SolderingUser)
                .Where(p => p.LabelPrintTs >= lampFilterDto.StartDate && p.LabelPrintTs < lampFilterDto.EndDate ||
                           p.AlProfileCutTs >= lampFilterDto.StartDate && p.AlProfileCutTs < lampFilterDto.EndDate ||
                           p.AlProfileDrillTs >= lampFilterDto.StartDate && p.AlProfileDrillTs < lampFilterDto.EndDate ||
                           p.LedModuleMountingTs >= lampFilterDto.StartDate && p.LedModuleMountingTs < lampFilterDto.EndDate ||
                           p.LightSolderingTs >= lampFilterDto.StartDate && p.LightSolderingTs < lampFilterDto.EndDate ||
                           p.LightAssemblingTs >= lampFilterDto.StartDate && p.LightAssemblingTs < lampFilterDto.EndDate ||
                           p.LightCheckingPackagingTs >= lampFilterDto.StartDate && p.LightCheckingPackagingTs < lampFilterDto.EndDate
                       )
                .ToListAsync();

            return View(products.Select(LedLampDto.FromLedLamp));

        }
    }
}
