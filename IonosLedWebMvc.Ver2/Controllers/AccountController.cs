using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Infrastructure;
using IonosLedWebMvc.Ver2.Services;
using IonosLedWebMvc.Ver2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IonosLedWebMvc.Ver2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserAuthService _userAuthService;
        private const string IS_SAVED_SESSION = "IsSavedSession";

        public AccountController(UserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel regModel)
        {
            if (ModelState.IsValid) {
                var (isSuccess, msg) = await _userAuthService.CreateAsync(regModel);
                if (isSuccess) {
                    ViewBag.message = msg;
                    return View();
                }
                else {
                    ModelState.AddModelError("", msg);
                }
            }
            return View(regModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid) {
                var (currUserAuth, errorMsg) = await _userAuthService.GetUserAuthAsync(loginModel);
                if (currUserAuth != null) {

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
                ModelState.AddModelError(string.Empty, errorMsg);
            }
            return View(loginModel);
        }


        public async Task<IActionResult> Logout()
        {
            // Clear the user's session
            /*            HttpContext.Session.Clear();*/
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated) {
               /* if (HttpContext.User.Claims.Any(c => c.Type == IS_SAVED_SESSION && c.Value == "true")) {*/
                    HttpContext.Session.Clear();
                //}
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

        public string AccessDenied()
        {
            return "Access denied";
        }
    }
}
