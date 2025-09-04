using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using bikamovie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bikamovie.Controllers
{
    public class AccountController : Controller
    {
        private readonly FilmDbContext _context;

        public AccountController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Şifre alanı boş bırakılamaz.");
                return View(model);
            }

            var passwordHash = ComputeSha256Hash(model.Password);
            var user = _context.Users
                .FirstOrDefault(u =>
                    !string.IsNullOrEmpty(u.Username) &&
                    !string.IsNullOrEmpty(u.PasswordHash) &&
                    u.Username == model.Username &&
                    u.PasswordHash == passwordHash);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                "CookieScheme",
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = false
                });

            return RedirectToAction("Profile", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieScheme");
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("", "Bu kullanıcı adı zaten alınmış.");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                PasswordHash = ComputeSha256Hash(model.Password),
                Email = model.Email
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == userName);

            if (user != null)
            {
                var favoriteFilms = _context.Favorites
                    .Where(f => f.UserId == user.Id)
                    .Select(f => f.Film)
                    .ToList();

                var model = new ProfileModel
                {
                    UserName = user.Username,
                    Email = user.Email,
                    FavoriteFilms = favoriteFilms
                };

                return View(model);
            }

            return RedirectToAction("Login");
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}