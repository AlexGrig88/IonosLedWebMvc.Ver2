using IonosLedWebMvc.Ver2.Dtos;

namespace IonosLedWebMvc.Ver2.Models
{
    public class EmployeeWithUserAuth
    {
        public EmployeeDto EmpDto { get; set; } = new EmployeeDto();
        public UserAuthWithAccessDto UserAuthWithAccessDto { get; set; } = new UserAuthWithAccessDto();
        public IEnumerable<EmployeeDto>? EmployeeList { get; set; }
        public IEnumerable<UserAuthWithAccessDto>? UserAuthList { get; set; }
    }
}
