namespace IonosLedWebMvc.Ver2.Models
{
    public class StatisticModel
    {
        public Dictionary<DateTime, int> DateToCountDict { get; set; }
        public Dictionary<string, int> LampModelToCountDict { get; set; }
    }
}
