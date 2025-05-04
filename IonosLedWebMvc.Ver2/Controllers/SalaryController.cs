using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Infrastructure;
using IonosLedWebMvc.Ver2.Models;
using IonosLedWebMvc.Ver2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class SalaryController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly LampService _lampService;
        private readonly SalaryService _salaryService;
        private const string ALL_EMPLOYEES = "Все сотрудники";
        private const int DAY_OF_SALARY = 5;

        public SalaryController(ApplicationContext context, LampService lampService, SalaryService salaryService)
        {
            _context = context;
            _lampService = lampService;
            _salaryService = salaryService;
        }

        public async Task<IActionResult> Index(string? startDate, string? endDate, string? employeeName)
        {
            ViewBag.FirstEnter = true;

            employeeName ??= ALL_EMPLOYEES;

            var allEmployees = await _context.Users.Select(u => u.Name).ToListAsync();
            if (employeeName == ALL_EMPLOYEES) {
                allEmployees.Insert(0, employeeName);
            }
            else {
                HelperFunctions.Swap<string>(allEmployees, 0, allEmployees.FindIndex(emp => emp == employeeName));
                allEmployees.Add(ALL_EMPLOYEES);
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
            if (startDate == null && endDate == null) {
                return View(new List<LedLampDto>());
            }

            var lamps = _lampService.GetLampsTimeFiltering(startDt, endDt);

            if (employeeName != ALL_EMPLOYEES) {
                lamps = _lampService.GetLampsTimeAndEmployeeFiltering(startDt, endDt, employeeName);
            }
            var lampList = await lamps.ToListAsync();

            List<EmployeeSalary> salaries = _salaryService.CalculateSalary(lampList, startDt, endDt);
            if (employeeName == ALL_EMPLOYEES) {
                ViewBag.Salaries = salaries;
            }
            else {
                ViewBag.Salaries = salaries.Where(s => s.Name.Contains(employeeName)).ToList();
            }

            var lampDtoList = lamps.Select(l => LedLampDto.FromLedLamp(l)).ToList();
            ViewBag.TotalRecords = lampList.Count;
            return View(lampDtoList.Take(200).ToList());

        }
    }
}
