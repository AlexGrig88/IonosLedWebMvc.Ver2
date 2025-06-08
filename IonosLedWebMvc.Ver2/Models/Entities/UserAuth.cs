using IonosLedWebMvc.Ver2.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace IonosLedWebMvc.Ver2.Models.Entities
{
    public class UserAuth
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
        public string? PasswordHash { get; set; }
        public string? Phone { get; set; }
        [Required]
        public EnumStorage.Group ProdGroup { get; set; } = EnumStorage.Group.DRONE;
        [Required]
        public EnumStorage.UserRole URole { get; set; } = EnumStorage.UserRole.EMPLOYEER;

        public byte[]? AvatarImg { get; set; }

        public DateOnly? BirthDate { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        public AccessRights? AccessRights { get; set; }

        public UserAuth() { }

        public UserAuth(string firstName, string lastName, string username, string email, string? passwordHash)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            RegisterDate = DateTime.Now;
            ProdGroup = EnumStorage.Group.DRONE;
            URole = EnumStorage.UserRole.EMPLOYEER;
            AccessRights = new AccessRights()
            {
                AccessRoom1 = false,
                AccessRoom2 = false,
                AccessRoom3 = false,
                AccessRoom4 = false,
                AccessRoom5 = false,
                AccessRoom6 = false,
                AccessRoom7 = false,
                AccessRoom8 = false
            };
        }
    }
}
    