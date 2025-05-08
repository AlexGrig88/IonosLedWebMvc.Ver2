using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models;
using Microsoft.EntityFrameworkCore;

namespace IonosLedWebMvc.Ver2.Services
{
    public class UserEventsService
    {
        private const string EVENT_NOT_FOUND = "Не был за последний месяц";
        private const string EVENT_IN_SYSTEM = "Зашел";
        private const string EVENT_OUT_SYSTEM = "Вышел";
        private DateTime DATE_NOW_EVENING_TEST = new DateTime(2024, 11, 18, 6, 0, 0);   //заменить на локальный DateTime.Now
        private readonly ApplicationContext _context;

        public UserEventsService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<uint, string>> GetStatusForLastMonth(List<User> users)
        {
            // получаем записи по событиям пользователя за последние 31 день
            var lastMonth = DATE_NOW_EVENING_TEST.Subtract(new TimeSpan(31, 0, 0, 0));    // для теста стоит последний день из базы !!! поменять на DateTime.Now
            var userEvents = await _context.UserEvents.Where(e => e.Ts > lastMonth).OrderByDescending(e => e.Ts).ToListAsync();
            Dictionary<uint, string> dictUserIdToStatus = new Dictionary<uint, string>();
            foreach (var user in users)
            {
                var currEvent = userEvents.FirstOrDefault(e => e.UserId == user.Id);
                if (currEvent != null) {
                    
                    string status = currEvent.Event == "LOGIN" ? EVENT_IN_SYSTEM :
                        currEvent.Event == "LOGOUT" ? EVENT_OUT_SYSTEM :
                        currEvent.Event;
                    string ts = currEvent.Ts.Date == DATE_NOW_EVENING_TEST.Date? $"в {currEvent.Ts:t}" : $"{currEvent.Ts:g}";
                    dictUserIdToStatus[user.Id] = $"{status} {ts}";
                }
                else {
                    dictUserIdToStatus[user.Id] = EVENT_NOT_FOUND;
                }
            }
            return dictUserIdToStatus;
        }
    }
}
