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
			DateTime startDate = selectedRange switch
			{
				1 => DATE_NOW_FAKE_TEST.Subtract(new TimeSpan(30, 0, 0, 0)),
				2 => DATE_NOW_FAKE_TEST.Subtract(new TimeSpan(92, 0, 0, 0)),
				3 => DATE_NOW_FAKE_TEST.Subtract(new TimeSpan(183, 0, 0, 0)),
				4 => DATE_NOW_FAKE_TEST.Subtract(new TimeSpan(365, 0, 0, 0)),
				_ => DATE_NOW_FAKE_TEST,
			};
			ViewData["SelectedRange"] = selectedRange;
			ViewData["StartDate"] = $"{startDate:d}";
			ViewData["EndDate"] = $"{DATE_NOW_FAKE_TEST:d}";
			var dayToCountLamp = await _statisticsService.GetMapDaysToCountLampAsync(startDate);
            return View(dayToCountLamp);
        }
    }
}
