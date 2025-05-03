using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Infrastructure;
using IonosLedWebMvc.Ver2.Models;
using IonosLedWebMvc.Ver2.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class SalaryController : Controller
    {

        private readonly ApplicationContext _context;
        private readonly ILampRepo _lampRepository;
        private const int PAGE_SIZE = 10;
        private const string ALL_EMPLOYEES = "Все сотрудники";
        private const string ALL_MODELS = "Все модели";

        public SalaryController(ApplicationContext context, ILampRepo lampRepo)
        {
            _context = context;
            _lampRepository = lampRepo;
        }

        public async Task<IActionResult> IndexGetPost(string? startDate, string? endDate, string? employeeName, string? modelName, int pageNumber)
        {

            var correctParameters = await GetCorrectFilterParameters(startDate, endDate, employeeName, modelName, pageNumber);

            pageNumber = correctParameters.PageNumber;
            ViewBag.AllEmployees = correctParameters.EmployeeNames;
            ViewBag.AllModels = correctParameters.ModelNames;

            employeeName ??= ALL_EMPLOYEES;
            modelName ??= ALL_MODELS;
            ViewBag.EmployeeName = employeeName;
            ViewBag.ModelName = modelName;

            var lamps = _lampRepository.GetAllAsync();
            DateTime startDt = correctParameters.StartDt;
            DateTime endDt = correctParameters.EndDt;

            ViewBag.StartDate = $"{startDt:s}";
            ViewBag.EndDate = $"{endDt:s}";


            if (startDt > endDt) {
                return View();
            }

            lamps = lamps.Where(p => p.LabelPrintTs >= startDt && p.LabelPrintTs < endDt ||
                            p.AlProfileCutTs >= startDt && p.AlProfileCutTs < endDt ||
                            p.AlProfileDrillTs >= startDt && p.AlProfileDrillTs < endDt ||
                            p.LedModuleMountingTs >= startDt && p.LedModuleMountingTs < endDt ||
                            p.LightSolderingTs >= startDt && p.LightSolderingTs < endDt ||
                            p.LightAssemblingTs >= startDt && p.LightAssemblingTs < endDt ||
                            p.LightCheckingPackagingTs >= startDt && p.LightCheckingPackagingTs < endDt
                        );

            if (employeeName != ALL_EMPLOYEES) {
                lamps = lamps.Where(p => (p.LabelPrintUser != null && p.LabelPrintUser.Name == employeeName) ||
                            (p.CutUser != null && p.CutUser.Name == employeeName) ||
                            (p.DrillUser != null && p.DrillUser.Name == employeeName) ||
                            (p.MountingUser != null && p.MountingUser.Name == employeeName) ||
                            (p.AssemblingUser != null && p.AssemblingUser.Name == employeeName) ||
                            (p.SolderingUser != null && p.SolderingUser.Name == employeeName) ||
                            (p.CheckingPackagingUser != null && p.CheckingPackagingUser.Name == employeeName)
                            );
            }

            if (modelName != ALL_MODELS) {
                lamps = lamps.Where(p => p.Model != null && p.Model.ModelName == modelName);
            }


            var lampsPaginated = await PaginatedList<LedLamp>.CreateAsync(lamps, pageNumber, PAGE_SIZE);

            // для расчета полученного числа записей необходимо получить значение колличества записей на последней странице, т.к. она может не полная
            var correctPageNumber = lampsPaginated.TotalPages < 1 ? 1 : lampsPaginated.TotalPages;
            var lastPage = await PaginatedList<LedLamp>.CreateAsync(lamps, correctPageNumber, PAGE_SIZE);
            var totalRecords = lampsPaginated.TotalPages * PAGE_SIZE - (PAGE_SIZE - lastPage.Items.Count);
            ViewBag.TotalRecords = totalRecords < 0 ? 0 : totalRecords;

            return View(lampsPaginated);
        }

        public async Task<FilterParametersLedLamp> GetCorrectFilterParameters(string? startDate, string? endDate, string? employeeName, string? modelName, int pageNumber)
        {
            int correctPageNumber = pageNumber < 1 ? 1 : pageNumber;

            var allEmployees = await _context.Users.Select(u => u.Name).ToListAsync();
            var allModels = await _context.LampModels.Select(m => m.ModelName).ToListAsync();
            if (employeeName == null || employeeName == ALL_EMPLOYEES) {
                employeeName = ALL_EMPLOYEES;
                allEmployees.Insert(0, employeeName);
            }
            else {
                HelperFunctions.Swap(allEmployees, 0, allEmployees.FindIndex(emp => emp == employeeName));
                allEmployees.Add(ALL_EMPLOYEES);
            }

            if (modelName == null || modelName == ALL_MODELS) {
                modelName = ALL_MODELS;
                allModels.Insert(0, modelName);
            }
            else {
                HelperFunctions.Swap(allModels, 0, allModels.FindIndex(m => m == modelName));
                allModels.Add(ALL_MODELS);
            }
            DateTime startDt = DateTime.Now;
            DateTime endDt = DateTime.Now;

            if (startDate == null && endDate == null) {
                startDt = new DateTime(startDt.Year, startDt.Month, startDt.Day, startDt.Hour, startDt.Minute, 0); 
                endDt = new DateTime(startDt.Year, startDt.Month, startDt.Day, startDt.Hour, startDt.Minute, 0);

            }
            else if (startDate == null) {
                startDt = new DateTime(2022, 2, 22);
                endDt = DateTime.Parse(endDate);
            }
            else if (endDate == null) { 
                endDt = new DateTime(startDt.Year, startDt.Month, startDt.Day, startDt.Hour, startDt.Minute, 0);
                startDt = DateTime.Parse(startDate);
            }
            else {
                endDt = DateTime.Parse(endDate);
                startDt = DateTime.Parse(startDate);
            }
            return new FilterParametersLedLamp() { StartDt = startDt, EndDt = endDt, EmployeeNames = allEmployees, ModelNames = allModels, PageNumber = correctPageNumber };
            
        }


        public IActionResult Index()
        {
            var filter = new LampFilterDto() { StartDate = new DateTime(2024, 4, 20), EndDate = new DateTime(2024, 04, 25) };
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
