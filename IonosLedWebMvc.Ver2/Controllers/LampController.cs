using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Dtos;
using IonosLedWebMvc.Ver2.Infrastructure;
using IonosLedWebMvc.Ver2.Models;
using IonosLedWebMvc.Ver2.Models.Entities;
using IonosLedWebMvc.Ver2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class LampController : Controller
    {

        private readonly ApplicationContext _context;
        private readonly LampService _lampService;
        private int page_size_dynamic = 20;
        private const string ALL_EMPLOYEES = "Все сотрудники";
        private const string ALL_MODELS = "Все модели";

        private DateTime DATE_NOW_FAKE_TEST = new DateTime(2024, 10, 30, 17, 32, 0); // для теста ставим текущим последний день в базе, потом вернуть DateTime.Now

        public LampController(ApplicationContext context, LampService lampService)
        {
            _context = context;
            _lampService = lampService;
        }

        // string? startDate, string? endDate, string? employeeName, string? modelName, int pageNumber, bool checkToday, uint? bitrixSearch
        public async Task<IActionResult> IndexGetPost(IndexLampParameter parameter)
        {
            string? startDate = parameter.StartDate;
            string? endDate = parameter.EndDate;
            string? employeeName = parameter.EmployeeName;
            string? modelName = parameter.ModelName;
            int pageNumber = parameter.PageNumber;
            bool checkToday = parameter.CheckToday;
            uint? bitrixSearch = parameter.BitrixSearch;
            page_size_dynamic = parameter.RecordsCount == 0 ? page_size_dynamic : parameter.RecordsCount;
            ViewData["RecordsCount"] = page_size_dynamic;

            ViewData["CurrentSort"] = parameter.OrderSerial;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(parameter.OrderSerial) ? "serial_desc" : "";
           

            ViewBag.SelectedOperation = parameter.SelectedOperation ?? "all";
            string selectedOperation = parameter.SelectedOperation ?? "all";

            var correctParameters = await GetCorrectFilterParameters(startDate, endDate, employeeName, modelName, pageNumber);

            pageNumber = correctParameters.PageNumber;
            ViewBag.AllEmployees = correctParameters.EmployeeNames;
            ViewBag.AllModels = correctParameters.ModelNames;

            employeeName ??= ALL_EMPLOYEES;
            modelName ??= ALL_MODELS;
            ViewBag.EmployeeName = employeeName;
            ViewBag.ModelName = modelName;


            DateTime startDt = correctParameters.StartDt;
            DateTime endDt = correctParameters.EndDt;

            if (bitrixSearch.HasValue) {
                startDt = new DateTime(2023, 1, 1);
                var now = DateTime.Now;
                endDt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0); 
            }
            else if (checkToday) {
                endDt = DATE_NOW_FAKE_TEST;
                startDt = DATE_NOW_FAKE_TEST.Date;
            }

            ViewBag.StartDate = $"{startDt:s}";
            ViewBag.EndDate = $"{endDt:s}";
            ViewBag.Check = checkToday;


            if (startDt > endDt) {
                return View();
            }

            if (startDate == null && endDate == null && !bitrixSearch.HasValue) {
                return View(new PaginatedList<LedLamp>(true));
            }

            IQueryable<LedLamp> lamps = null;

            if (bitrixSearch.HasValue) {
                lamps = _lampService.GetLampsSearchBitrixNum(bitrixSearch.Value);
                ViewBag.BitrixNum = bitrixSearch.Value.ToString();
            }
            else {
                ViewBag.BitrixNum = null;
                lamps = selectedOperation switch
                {
                    "all" => _lampService.GetLampsTimeFiltering(startDt, endDt),
                    "print" => _lampService.GetLabelPrintTimeFiltering(startDt, endDt),
                    "cut" => _lampService.GetCutTimeFiltering(startDt, endDt),
                    "drill" => _lampService.GetDrillTimeFiltering(startDt, endDt),
                    "mount" => _lampService.GetMountTimeFiltering(startDt, endDt),
                    "solder" => _lampService.GetSolderTimeFiltering(startDt, endDt),
                    "assembly" => _lampService.GetAssemblyTimeFiltering(startDt, endDt),
                    "packeg" => _lampService.GetPackegTimeFiltering(startDt, endDt),
                    _ => throw new NotImplementedException("Ошибка!!!, проверить!")
                };

                if (modelName != ALL_MODELS && employeeName != ALL_EMPLOYEES) {
                    lamps = _lampService.GetLampsTimeAndEmployeeAndModelFiltering(startDt, endDt, employeeName, modelName, selectedOperation);
                }
                else if (employeeName != ALL_EMPLOYEES) {
                    lamps = _lampService.GetLampsTimeAndEmployeeFiltering(startDt, endDt, employeeName, selectedOperation);
                }
                else if (modelName != ALL_MODELS) {
                    lamps = _lampService.GetLampsTimeAndAndModelFiltering(startDt, endDt, modelName, selectedOperation);
                }

                switch (parameter.OrderSerial) {
                    case "serial_desc":
                        lamps = lamps.OrderByDescending(p => p.Id);
                        break;
                    default:
                        lamps = lamps.OrderBy(p => p.Id); 
                        break;
                }
            }

            if (bitrixSearch.HasValue) page_size_dynamic = 50;

            // var lampsPaginated = await PaginatedList<LedLamp>.CreateAsync(lamps, pageNumber, page_size_dynamic);
            var lampsPaginated = await PaginatedList<LedLamp>.CreateAsync(lamps, pageNumber, page_size_dynamic);

            // для расчета полученного числа записей необходимо получить значение колличества записей на последней странице, т.к. она может не полная
            var lastPageNumber = lampsPaginated.TotalPages < 1 ? 1 : lampsPaginated.TotalPages;
            var lastPage = await PaginatedList<LedLamp>.CreateAsync(lamps, lastPageNumber, page_size_dynamic);
            var totalRecords = lampsPaginated.TotalPages * page_size_dynamic - (page_size_dynamic - lastPage.Items.Count);
            ViewBag.TotalRecords = totalRecords < 0 ? 0 : totalRecords;

            if (employeeName != ALL_EMPLOYEES) {
                lampsPaginated.LampDtoItems = lampsPaginated.Items.Select(l => new LedLampDto(l, employeeName)).ToList();
            }
            else {
                lampsPaginated.LampDtoItems = lampsPaginated.Items.Select(LedLampDto.FromLedLamp).ToList();
            }
            lampsPaginated.Items = null;
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
                HelperFunctions.Swap<string>(allEmployees, 0, allEmployees.FindIndex(emp => emp == employeeName));
                allEmployees.Add(ALL_EMPLOYEES);
            }

            if (modelName == null || modelName == ALL_MODELS) {
                modelName = ALL_MODELS;
                allModels.Insert(0, modelName);
            }
            else {
                HelperFunctions.Swap<string>(allModels, 0, allModels.FindIndex(m => m == modelName));
                allModels.Add(ALL_MODELS);
            }
            DateTime startDt = new DateTime(2024, 10, 30, 17, 32, 0);    // // для теста ставим текущим последний день в базе, потом вернуть DateTime.Now
            DateTime endDt = DateTime.Now;

            
            if (startDate == null && endDate == null) {
                // устанавливаем текущий день
                endDt = DATE_NOW_FAKE_TEST;
                startDt = DATE_NOW_FAKE_TEST.Date;
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


    }

}
