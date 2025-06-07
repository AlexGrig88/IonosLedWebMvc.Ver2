using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Infrastructure;
using IonosLedWebMvc.Ver2.Models.Entities;
using IonosLedWebMvc.Ver2.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace IonosLedWebMvc.Ver2.Services
{
    public class UserAuthService
    {
        private readonly ApplicationContext _context;

        public UserAuthService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<(bool isSucces, string msg)> CreateAsync(RegisterViewModel regModel)
        {
            UserAuth userAuth = new UserAuth(regModel.FirstName, regModel.LastName, regModel.Username, regModel.Email, SecretHasher.Hash(regModel.Password));
            _context.UserAuths.Add(userAuth);
            try {
                await _context.SaveChangesAsync();
                return (true, "Пользователь успешно зарегестрирован");
            }
            catch (DbUpdateConcurrencyException ex) {
                return(false, ex.Message);
            }
        }

        public async Task<(UserAuth? userAuth, string reasonNotFound)> GetUserAuthAsync(LoginViewModel loginModel)
        {
            var currUserAuth = await _context.UserAuths.FirstOrDefaultAsync(u => u.Email == loginModel.UserNameOrEmail || u.Username == loginModel.UserNameOrEmail);
            if (currUserAuth == null)
            {
                return (null, "Вход не выполнен. Проверьте правильность вводимых никнейма или email.");
            }
            else if (SecretHasher.Verify(loginModel.Password, currUserAuth.PasswordHash)) {
                return (null, "Пароль не верный. Попробуйте повторить попытку.");
            }
            else {
                return (currUserAuth, "Вы вошли в систему");
            }

        }
    }
}
