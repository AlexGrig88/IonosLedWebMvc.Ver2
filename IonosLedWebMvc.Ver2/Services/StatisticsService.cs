using IonosLedWebMvc.Ver2.Data;
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

		public async Task<Dictionary<DateTime, int>> GetMapDaysToCountLampAsync(DateTime startDate)
		{

			var lamps =  await _lampRepo.GetAllRealesedForThePeriodAsync(startDate, DATE_NOW_FAKE_TEST).Select(l => l.LightCheckingPackagingTs!.Value.Date).ToListAsync();
			var dayToCountList = lamps.GroupBy(g => g).Select(g => new { Day = g.Key, Count = g.Count()}).OrderBy(p => p.Day).ToList();
/*			int day = 1;
			var dict = new Dictionary<int, int>();
			foreach ( var item in dayToCountList) {
				dict[day++] = item.Count;
			}*/
			var dict = new Dictionary<DateTime, int>();
			foreach (var day in dayToCountList) {
				dict[day.Day] = day.Count;
			}
			return dict;
		}
	}
}
