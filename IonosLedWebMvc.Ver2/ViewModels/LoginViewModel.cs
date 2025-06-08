using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IonosLedWebMvc.Ver2.ViewModels
{
    public class LoginViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Введите username или email")]
        [MaxLength(50, ErrorMessage = "Длина не должна превышать 50 символов")]
        [DisplayName("Username или email")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Пароль не должен быть короче 4 символов и не более 20 символов")]
        [DataType(DataType.Password)]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        [DisplayName("Запомнить меня")]
        public bool IsSavedDession { get; set; }
    }
}
