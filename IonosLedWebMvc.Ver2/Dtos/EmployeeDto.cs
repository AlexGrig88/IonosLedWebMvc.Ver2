using IonosLedWebMvc.Ver2.Models;
using System.ComponentModel;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class EmployeeDto
    {
        public uint Id { get; set; }
        [DisplayName("Имя сотрудника")]
        public string Name { get; set; } = null!;
        [DisplayName("Пин")]
        public ushort Pin { get; set; }
        [DisplayName("Должность")]
        public string RoleName { get; set; } = string.Empty;

        public static EmployeeDto FromUser(User user) => new EmployeeDto() { Id = user.Id, Name = user.Name, Pin = user.Pin, RoleName = user.Role.RoleName};
        public static User ToUser(EmployeeDto emp)
        {
            var user = new User() { Id = emp.Id, Name = emp.Name, Pin = emp.Pin };
            return user;
        }

    }
}
