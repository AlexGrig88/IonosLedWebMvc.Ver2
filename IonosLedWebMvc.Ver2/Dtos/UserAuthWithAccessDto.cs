
using IonosLedWebMvc.Ver2.Infrastructure;
using IonosLedWebMvc.Ver2.Models.Entities;
using System.ComponentModel;

namespace IonosLedWebMvc.Ver2.Dtos
{
    public class UserAuthWithAccessDto
    {
        public int Id { get; set; }
        [DisplayName("Имя")]
        public string FirstName { get; set; } = string.Empty;
        [DisplayName("Фамилия")]
        public string LastName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;
 
        public string Email { get; set; } = string.Empty;

        [DisplayName("Группа")]
        public EnumStorage.Group ProdGroup { get; set; }
        [DisplayName("Роль")]
        public EnumStorage.UserRole URole { get; set; }
        [DisplayName("Дата регистрации")]
        public DateTime RegisterDate { get; set; }

        [DisplayName("Аватар")]
        public byte[] AvatarImg { get; set; }

        [DisplayName("Телефон")]
        public string? Phone { get; set; }

        public List<bool> RoomAccesses { get; set; } = new List<bool>();

        public static UserAuthWithAccessDto FromUserAuthWithAccess(UserAuth user)
        {
            if (user.AccessRights == null) throw new InvalidOperationException("Ожидается, что мы получаем сущность пользователя с присоединёнными правами доступа. Но этого не случилось. Ищи ошибку");
            return new UserAuthWithAccessDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                ProdGroup = user.ProdGroup,
                URole = user.URole,
                RegisterDate = user.RegisterDate,
                AvatarImg = user.AvatarImg,
                Phone = user.Phone,
                RoomAccesses = new List<bool> {
                    user.AccessRights.AccessRoom1, user.AccessRights.AccessRoom2, user.AccessRights.AccessRoom3,
                    user.AccessRights.AccessRoom4, user.AccessRights.AccessRoom5, user.AccessRights.AccessRoom6,
                    user.AccessRights.AccessRoom7, user.AccessRights.AccessRoom8
                }
            };
        } 
    }
}
