using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace IonosLedWebMvc.Ver2.Services
{
    public class UserEventsService
    {
        private const string EVENT_NOT_FOUND_MONTH = "Не был за последний месяц";
        private const string EVENT_NOT_FOUND_WEEK = "Не был за последнюю неделю";
        private const string EVENT_IN_SYSTEM = "Зашел";
        private const string EVENT_OUT_SYSTEM = "Вышел";
        private DateTime DATE_NOW_EVENING_TEST = new DateTime(2024, 11, 18, 6, 0, 0);   //заменить на локальный DateTime.Now
        private readonly ApplicationContext _context;

        public UserEventsService(ApplicationContext context)
        {
            _context = context;
        }

        // получение событий каждого пользователя за указанный период до данного момента
        public async Task<Dictionary<uint, string>> GetLastEventFor(TimeSpan ts, List<User> users)
        {
            // получаем записи по событиям пользователя за последнее время
            var lastTs = DATE_NOW_EVENING_TEST.Subtract(ts);    // для теста стоит последний день из базы !!! поменять на DateTime.Now
            var userEvents = await _context.UserEvents.Where(e => e.Ts > lastTs).OrderByDescending(e => e.Ts).ToListAsync();
            Dictionary<uint, string> dictUserIdToStatus = new Dictionary<uint, string>();
            foreach (var user in users)
            {
                var currEvent = userEvents.FirstOrDefault(e => e.UserId == user.Id);
                if (currEvent != null) {
                    
                    string status = currEvent.Event == "LOGIN" ? EVENT_IN_SYSTEM :
                        currEvent.Event == "LOGOUT" ? EVENT_OUT_SYSTEM :
                        currEvent.Event;
                    string currTs = currEvent.Ts.Date == DATE_NOW_EVENING_TEST.Date? $"в {currEvent.Ts:t}" : $"{currEvent.Ts:g}";
                    dictUserIdToStatus[user.Id] = $"{status} {currTs}";
                }
                else {
                    dictUserIdToStatus[user.Id] = EVENT_NOT_FOUND_MONTH;
                }
            }
            return dictUserIdToStatus;
        }

        public async Task<List<UserEvent>> GetAllEventsFor(TimeSpan ts, User user)
        {
            // получаем записи по событиям пользователя за последнее время
            var lastTs = DATE_NOW_EVENING_TEST.Subtract(ts);    // для теста стоит последний день из базы !!! поменять на DateTime.Now
            var userEvents = await _context.UserEvents
                .Where(e => e.Ts > lastTs && e.UserId == user.Id)
                .OrderByDescending(e => e.Ts)
                .ToListAsync();
            return userEvents;
        }
    }
}
