namespace IonosLedWebMvc.Ver2.Data
{
    public class EmployeeDto
    {
        public uint Id { get; set; }

        public string Name { get; set; } = null!;

        public ushort Pin { get; set; }

        public uint Role { get; set; }
    }
}
