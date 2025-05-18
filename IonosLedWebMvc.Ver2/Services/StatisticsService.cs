using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models;
using IonosLedWebMvc.Ver2.Repos;
using Microsoft.EntityFrameworkCore;

namespace IonosLedWebMvc.Ver2.Services
{
    public class StatisticsService
    {
        private readonly ILampRepo _lampRepo;
		private DateTime DATE_NOW_FAKE_TEST = new DateTime(2024, 11, 15, 18, 0, 0); // для теста ставим текущим последний день в базе, потом вернуть DateTime.Now

		public StatisticsService(ILampRepo lampRepo)
        {
            _lampRepo = lampRepo;
        }

		public async Task<StatisticModel> GetMapDaysToCountLampAsync(DateTime startDate)
		{

			var lamps =  await _lampRepo.GetAllRealesedForThePeriodAsync(startDate, DATE_NOW_FAKE_TEST).ToListAsync();
			var dayToCountList = lamps
				.GroupBy(g => g.LightCheckingPackagingTs!.Value.Date)
				.Select(g => new { Day = g.Key, Count = g.Count()})
				.OrderBy(p => p.Day)
				.ToList();
			var dictDate = new Dictionary<DateTime, int>();
			foreach (var item in dayToCountList) {
                dictDate[item.Day] = item.Count;
			}

            var lampModelToCountList = lamps
				.Where(l => l.Model != null && l.Model.ModelName != null)
				.GroupBy(g => g.Model!.ModelName)
				.Select(g => new { ModelName = g.Key, Count = g.Count() })
				.OrderByDescending(p => p.Count)
				.ToList();
			var dictModelName = new Dictionary<string, int>();
			foreach (var item in lampModelToCountList) {
				dictModelName[item.ModelName] = item.Count;
			}

            return new StatisticModel() { DateToCountDict = dictDate, LampModelToCountDict = dictModelName };
		}
	}
}
