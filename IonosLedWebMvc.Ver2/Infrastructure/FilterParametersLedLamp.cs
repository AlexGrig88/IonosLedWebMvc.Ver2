namespace IonosLedWebMvc.Ver2.Infrastructure
{
    public struct FilterParametersLedLamp
    {
        public DateTime StartDt { get; set; }
        public DateTime EndDt { get; set; }
        public List<string> EmployeeNames { get; set; }
        public List<string> ModelNames { get; set; }
        public int PageNumber { get; set; }
    }
}
