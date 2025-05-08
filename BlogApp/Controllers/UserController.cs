using System.Security.Claims;
using AutoMapper;
using BlogApp.Data.Abstract.IRepository;
using BlogApp.Entity;
using BlogApp.Helpers;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Post");
            }
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.Users().FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
                if (user != null)
                {
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Email, user.Email!.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName!.ToString()),
                        new Claim(ClaimTypes.UserData, user.Image!.ToString()),
                    };

                    if (user.Email == "fikriyenur@gmail.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Post");
                }
                TempData["LoginErrorMessage"] = "Şifre veya E-Posta hatalı! Lütfen tekrar deneyin.";
            }
            return View("Login", model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var userInDb = _userRepository.Users().FirstOrDefault(x => x.Email == model.Email || x.UserName == model.UserName);
                if (userInDb != null)
                {
                    TempData["RegisterErrorMessage"] = "Bu E-Posta veya kullanıcı adı zaten kullanılmakta!";
                    return View("Register", model);
                }

                var user = _mapper.Map<User>(model);
                user.Image = await SaveImageHelper.SaveImageAsync(model.ImageFile!);
                await _userRepository.AddUserAsync(user);
                TempData["RegisterMessage"] = "Kayıt işlemi başarılı! Giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }
            return View("Register", model);
        }

        [HttpGet("Profile/{userName}")]
        public async Task<IActionResult> Profile(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                TempData["UserNotFoundMessage"] = "Kullanıcı bulunamadı!";
                return RedirectToAction("Index", "Post");
            }

            var user = await _userRepository.Users()
                            .Include(x => x.Posts)
                            .Include(x => x.Comments)
                            .ThenInclude(x => x.Post)
                            .FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null)
            {
                TempData["UserNotFoundMessage"] = "Kullanıcı bulunamadı!";
                return RedirectToAction("Index", "Post");
            }

            var model = _mapper.Map<UserModel>(user);
            return View(model);
        }
    }
}