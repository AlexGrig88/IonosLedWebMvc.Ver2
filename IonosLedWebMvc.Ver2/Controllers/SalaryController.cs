using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Infrastructure;
using IonosLedWebMvc.Ver2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IonosLedWebMvc.Ver2.Controllers
{
    [Authorize]
    public class SalaryController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationContext _context;
        private readonly LampService _lampService;
        private readonly SalaryService _salaryService;
/*        private const string ALL_EMPLOYEES = "Все сотрудники";*/
        private const int DAY_OF_SALARY = 5;

        private static string _fileNameForExcel = "";

        public SalaryController(ApplicationContext context, LampService lampService, SalaryService salaryService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _lampService = lampService;
            _salaryService = salaryService;
            _environment = hostEnvironment;

        }

        public async Task<IActionResult> Index(string? startDate, string? endDate, string? employeeName, int? showLampsWithoutModel)
        {

/*            employeeName ??= ALL_EMPLOYEES;*/

            var allEmployees = await _context.Users.Select(u => u.Name).ToListAsync();
            if (!string.IsNullOrEmpty(employeeName)) {
                HelperFunctions.Swap<string>(allEmployees, 0, allEmployees.FindIndex(emp => emp == employeeName));
            }

            ViewBag.AllEmployees = allEmployees;
            ViewBag.EmployeeName = employeeName;

            DateTime startDt = DateTime.Now;
            DateTime endDt = DateTime.Now;
            if (startDate == null && endDate == null) {
                int currentMonth = startDt.Month;  // текущий месяц равен 5 числу либо текущего месяца
                int currentDay = DAY_OF_SALARY;    // либо 5 числу предыдущего месяца, если номер дня меньше 5
                if (startDt.Day < currentDay) {
                    currentMonth -= 1;
                }
                startDt = new DateTime(startDt.Year, currentMonth, currentDay, 0, 0, 0);
                endDt = new DateTime(endDt.Year, endDt.Month, endDt.Day, endDt.Hour, endDt.Minute, 0);

            }
            else if (startDate == null) {
                startDt = new DateTime(startDt.Year, startDt.Month, startDt.Day, startDt.Hour, startDt.Minute, 0);
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

            ViewBag.StartDate = $"{startDt:s}";
            ViewBag.EndDate = $"{endDt:s}";

            ViewBag.StartDateG = $"{startDt:g}";
            ViewBag.EndDateG = $"{endDt:g}";

            if (startDt > endDt) {
                return View();
            }
            if (endDt - startDt > new TimeSpan(62, 0, 0, 0)) {
                return View();
            }

            if (string.IsNullOrEmpty(employeeName) || (startDate == null && endDate == null)) {
                return View(new List<LedLampDto>());
            }

            var lampList = await _lampService.GetLampsTimeAndEmployeeFiltering(startDt, endDt, employeeName, "all").ToListAsync();
            ViewBag.TotalRecords = lampList.Count;

            var tuple = _salaryService.CalculateSalary(lampList, startDt, endDt);

            var empSalary = tuple.employeeSalaryList.FirstOrDefault(s => s.Name.Contains(employeeName));
            ViewBag.AmployeeAndSalary = empSalary;


            List<LedLampDto>? lampDtoList = null;

            if (showLampsWithoutModel.HasValue && showLampsWithoutModel.Value == 1) {
                //lampDtoList = tuple.lampWithoutModelList.Select(LedLampDto.FromLedLamp).ToList();
                lampDtoList = tuple.lampWithoutModelList.Select(l => new LedLampDto(l, employeeName)).ToList();
            }
            else {
                //lampDtoList = lampList.Select(LedLampDto.FromLedLamp).ToList();
                lampDtoList = lampList.Select(l => new LedLampDto(l, employeeName)).ToList();
            }
            //////////////////////////////////////////////
            // создание файла excel с детализацией
            string path = Path.Combine(_environment.WebRootPath, "ExcelFilesDir");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            _fileNameForExcel = Path.Combine(path, "Details_For_" + employeeName + ".xlsx");
            ExcelCreator.GererateAndSaveFile(lampDtoList, _fileNameForExcel, employeeName, empSalary.Salary, startDt, endDt);
            ///////////////////////////////////////////////////////


            showLampsWithoutModel = tuple.lampWithoutModelList.Count > 0 ? 1 : 0;
            ViewBag.TotalRecordsWithoutModel = tuple.lampWithoutModelList.Count;
            ViewBag.ShowLampsWithoutModel = showLampsWithoutModel;
            

            return View(lampDtoList.Take(200).ToList());


        }

        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string fullpath = _fileNameForExcel;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(fullpath);

            fileName = "Detail_" + fileName + "_" + DateTime.Now.ToString().Replace('.', '_') + ".xlsx"; 
            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}
