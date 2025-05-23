using IonosLedWebMvc.Ver2.Services;
using Microsoft.AspNetCore.Mvc;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly StatisticsService _statisticsService;
		private DateTime DATE_NOW_FAKE_TEST = new DateTime(2024, 11, 15, 18, 0, 0); // для теста ставим текущим последний день в базе, потом вернуть DateTime.Now

		public StatisticsController(StatisticsService statisticsService)
        { 
            _statisticsService = statisticsService;
        }

        public async Task<IActionResult> Index(int selectedRange)
        {
            selectedRange = selectedRange == 0 ? 1 : selectedRange;
			var startDate = SelectStartDate(selectedRange);

			ViewData["SelectedRange"] = selectedRange;
			ViewData["StartDate"] = $"{startDate:d}";
			ViewData["EndDate"] = $"{DATE_NOW_FAKE_TEST:d}";
			var dateToLampCount = await _statisticsService.GetMapDaysToCountLampAsync(startDate);
            return View(dateToLampCount);
        }

		public async Task<IActionResult> GistoLampModels(int selectedRange)
		{
            selectedRange = selectedRange == 0 ? 1 : selectedRange;
            var startDate = SelectStartDate(selectedRange);
            ViewData["SelectedRange"] = selectedRange;
            ViewData["StartDate"] = $"{startDate:d}";
            ViewData["EndDate"] = $"{DATE_NOW_FAKE_TEST:d}";
			var modelNameToCount = await _statisticsService.GetMapLampModelToCountAsync(startDate);
			return View(modelNameToCount);
        }


		private DateTime SelectStartDate(int selectedRange)
		{
			return selectedRange switch
			{
				1 => DATE_NOW_FAKE_TEST.Subtract(new TimeSpan(7, 0, 0, 0)),
				2 => DATE_NOW_FAKE_TEST.Subtract(new TimeSpan(30, 0, 0, 0)),
				3 => DATE_NOW_FAKE_TEST.Subtract(new TimeSpan(61, 0, 0, 0)),
				4 => DATE_NOW_FAKE_TEST.Subtract(new TimeSpan(92, 0, 0, 0)),
				5 => DATE_NOW_FAKE_TEST.Subtract(new TimeSpan(122, 0, 0, 0)),
				6 => DATE_NOW_FAKE_TEST.Subtract(new TimeSpan(153, 0, 0, 0)),
				7 => DATE_NOW_FAKE_TEST.Subtract(new TimeSpan(183, 0, 0, 0)),
				8 => DATE_NOW_FAKE_TEST.Subtract(new TimeSpan(365, 0, 0, 0)),
				_ => DATE_NOW_FAKE_TEST,
			};
		}
    }
}
