namespace IonosLedWebMvc.Ver2.Models
{
    public class IndexLampParameter
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? EmployeeName { get; set; }
        public string? ModelName { get; set; }
        public int PageNumber { get; set; }
        public bool CheckToday { get; set; }
        public uint? BitrixSearch {  get; set; } 
        public int RecordsCount { get; set; }

        public string? SelectedOperation { get; set; }
        public string? OrderSerial { get; set; }
    }
}
