using IonosLedWebMvc.Ver2.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class UserAuthDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; }

        public string? Phone { get; set; }
        [Required]
        public EnumStorage.Group ProdGroup { get; set; }
        [Required]
        public EnumStorage.UserRole URole { get; set; }

        public byte[] AvatarImg { get; set; }

        public DateOnly BirthDate { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }
    }
}
