namespace IonosLedWebMvc.Ver2.Models
{
    public struct EmployeeSalary
    {
        public EmployeeSalary(string name, decimal? salary)
        {
            Name = name;
            Salary = salary;
        }

        public string Name { get; set; }
        public decimal? Salary { get; set; }
    }
}
