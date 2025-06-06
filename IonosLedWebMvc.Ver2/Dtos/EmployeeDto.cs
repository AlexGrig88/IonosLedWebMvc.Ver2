﻿using IonosLedWebMvc.Ver2.Models.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class EmployeeDto
    {
        public uint Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayName("Имя и фамилия сотрудника")]
        [RegularExpression(@"[А-Яа-я]+ [А-Яа-я]+", ErrorMessage = "Должны быть использованы только русские буквы алфавита и пробел")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 80 символов")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayName("Пин")]
        [Range(1000, 9999, ErrorMessage = "Пин должен быть целым четырехзначным числом от 1000 до 9999")]
        public ushort Pin { get; set; }
        [DisplayName("Должность")]
        public string RoleName { get; set; } = string.Empty;

        [DisplayName("Cтатус")]
        public string Status { get; set; } = string.Empty;

        public List<UserEventDto> UserEventsDto { get; set; } = new List<UserEventDto>();

        public static EmployeeDto FromUser(User user, Dictionary<uint, string>? userIdToEvent = null)
        {

            return new EmployeeDto() { 
                Id = user.Id,
                Name = user.Name,
                Pin = user.Pin,
                RoleName = user.Role.RoleName,
                Status = userIdToEvent?[user.Id] ?? string.Empty,
            };
        }

        public static EmployeeDto FromUserWithEvents(User user, List<UserEventDto> uEventsDto)
        {

            return new EmployeeDto()
            {
                Id = user.Id,
                Name = user.Name,
                Pin = user.Pin,
                RoleName = user.Role.RoleName,
                UserEventsDto = uEventsDto
            };
        }


        public static User ToUser(EmployeeDto emp)
        {
            var user = new User() { Id = emp.Id, Name = emp.Name, Pin = emp.Pin };
            return user;
        }

    }
}
