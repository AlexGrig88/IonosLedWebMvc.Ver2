using Microsoft.EntityFrameworkCore;

namespace IonosLedWebMvc.Ver2.Infrastructure
{
    public class FilterParametersLedLamp
    {
        public DateTime StartDt { get; set; }
        public DateTime EndDt { get; set; }
        public List<string> EmployeeNames { get; set; } = new List<string>();
        public List<string> ModelNames { get; set; } = new List<string>();
        public int PageNumber { get; set; }
        public bool CheckToday { get; set; }

        public async Task<FilterParametersLedLamp> GetCorrect(string? startDate, string? endDate, string? employeeName, string? modelName, int pageNumber)
        {
            throw new NotImplementedException();
        }
    }
}
