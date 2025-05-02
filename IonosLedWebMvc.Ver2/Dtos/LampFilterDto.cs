using System.ComponentModel;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class LampFilterDto
    {
        [DisplayName("Начало периода")]
        public DateTime? StartDate { get; set; } = GetDateWithTimeSetting(0, 0, 0);

        [DisplayName("Конец периода")]
        public DateTime? EndDate { get; set; } = GetDateWithTimeSetting(DateTime.Now.Hour, DateTime.Now.Minute, 0);

        [DisplayName("Сотрудник")]
        public string EmployeeName { get; set; } = "Все сотрудники";

        [DisplayName("Модель светильника")]
        public string ModelName { get; set; } = "Все модели";

        public static DateTime? GetDateWithTimeSetting(int hour, int minute, int second)
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, hour, minute, second);
        }
    }
}
