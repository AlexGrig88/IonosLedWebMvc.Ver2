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
    }
}
