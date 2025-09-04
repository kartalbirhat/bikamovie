using bikamovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace bikamovie.Controllers
{
    public class MovieController : Controller
    {
        private readonly FilmDbContext _context;

        public MovieController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Filmler(string? search)
        {
            var filmler = _context.Filmler.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var lowered = search.ToLower();
                filmler = filmler
                    .Where(f =>
                        (!string.IsNullOrEmpty(f.Ad) && f.Ad.ToLower().Contains(lowered)) ||
                        (!string.IsNullOrEmpty(f.Yonetmen) && f.Yonetmen.ToLower().Contains(lowered))
                    );

                var sonuc = filmler
                    .AsEnumerable()
                    .OrderBy(f =>
                        !string.IsNullOrEmpty(f.Ad) && f.Ad.ToLower().IndexOf(lowered) != -1
                            ? f.Ad.ToLower().IndexOf(lowered)
                            : int.MaxValue
                    )
                    .ThenBy(f => f.Ad)
                    .ToList();

                return View(sonuc);
            }

            var sonucDefault = filmler.OrderBy(f => f.Ad).ToList();
            return View(sonucDefault);
        }

        [HttpPost]
        public IActionResult Filmler(Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Filmler.Add(film);
                _context.SaveChanges();
                return RedirectToAction("Filmler");
            }
            var filmler = _context.Filmler.ToList();
            return View(filmler);
        }

        public IActionResult Detay(int id)
        {
            var film = _context.Filmler
                .Include(f => f.Comments)
                .FirstOrDefault(f => f.Id == id);

            var viewModel = new FilmDetayViewModel
            {
                Film = film
            };

            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var user = _context.Users.FirstOrDefault(u => u.Username == userName);
                if (user != null)
                {
                    viewModel.IsFavorite = _context.Favorites.Any(f => f.FilmId == id && f.UserId == user.Id);
                }
                else
                {
                    viewModel.IsFavorite = false;
                }
            }

            return View(viewModel);
        }

        public IActionResult Kategoriler(string? kategori)
        {
            var kategoriliFilmler = new Dictionary<string, List<Film>>();
            var filmler = _context.Filmler.ToList();

            foreach (var film in filmler)
            {
                if (string.IsNullOrWhiteSpace(film.Tur))
                    continue;

                var turler = film.Tur
                    .Split(',', ';')
                    .Select(t => t.Trim())
                    .Where(t => !string.IsNullOrEmpty(t));

                foreach (var tur in turler)
                {
                    if (!kategoriliFilmler.ContainsKey(tur))
                        kategoriliFilmler[tur] = new List<Film>();

                    kategoriliFilmler[tur].Add(film);
                }
            }

            if (!string.IsNullOrEmpty(kategori) && kategoriliFilmler.ContainsKey(kategori))
            {
                var filtered = new Dictionary<string, List<Film>>
                {
                    { kategori, kategoriliFilmler[kategori] }
                };
                return View(filtered);
            }

            var sorted = kategoriliFilmler.OrderBy(k => k.Key)
                                         .ToDictionary(k => k.Key, v => v.Value);

            return View(sorted);
        }

        public IActionResult Sil(int id)
        {
            var film = _context.Filmler.FirstOrDefault(f => f.Id == id);
            if (film != null)
            {
                _context.Filmler.Remove(film);
                _context.SaveChanges();
            }
            return RedirectToAction("Filmler");
        }

        [HttpPost]
        public IActionResult AddComment(int filmId, string text)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var userName = User.Identity.Name;
            var comment = new Comment
            {
                FilmId = filmId,
                UserName = userName,
                Text = text,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("Detay", new { id = filmId });
        }

        [HttpPost]
        public IActionResult AddToFavorites(int filmId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == userName);
            if (user == null)
                return RedirectToAction("Login", "Account");

            bool alreadyExists = _context.Favorites.Any(l => l.FilmId == filmId && l.UserId == user.Id);
            if (!alreadyExists)
            {
                var like = new Favorite
                {
                    FilmId = filmId,
                    UserId = user.Id,
                    UserName = userName
                };
                _context.Favorites.Add(like);
                _context.SaveChanges();
            }

            return RedirectToAction("Detay", new { id = filmId });
        }

        [HttpPost]
        public IActionResult RemoveFromFavorites(int filmId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == userName);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var favorite = _context.Favorites.FirstOrDefault(f => f.FilmId == filmId && f.UserId == user.Id);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
            }

            return RedirectToAction("Detay", new { id = filmId });
        }

        [HttpGet]
        public JsonResult SearchSuggestions(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(new List<string>());

            var results = _context.Filmler
                .Where(f => f.Ad.Contains(q))
                .Select(f => f.Ad)
                .Take(8)
                .ToList();

            return Json(results);
        }
    }
}
