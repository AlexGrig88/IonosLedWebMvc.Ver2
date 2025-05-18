using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;
        private const string IS_SAVED_SESSION = "IsSavedSession";

        public AccountController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid) {
                var currUserAuth = _context.UserAuths.FirstOrDefault(u => u.Email == loginModel.UserNameOrEmail || u.Username == loginModel.UserNameOrEmail);
                if (currUserAuth != null && currUserAuth.Password == loginModel.Password) {

                    var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, currUserAuth.Username),
                            new Claim(ClaimTypes.Role, "Admin"),
                            new Claim(IS_SAVED_SESSION, loginModel.IsSavedDession ? "true" : "false")
                            // add any additional claims
                        };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    if (loginModel.IsSavedDession) {
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    }
                    else {
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    }


                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Вход не выполнен. Проверьте вводимые данные.");
            }
            return View(loginModel);
        }


        public async Task<IActionResult> Logout()
        {
            // Clear the user's session
            /*            HttpContext.Session.Clear();*/
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated) {
/*                if (HttpContext.User.Claims.Any(c => c.Type == IS_SAVED_SESSION && c.Value == "true")) {
                    HttpContext.Session.Clear();
                }*/
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            

            // Redirect to the login page or home page
            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            return View();
        }
    }
}
