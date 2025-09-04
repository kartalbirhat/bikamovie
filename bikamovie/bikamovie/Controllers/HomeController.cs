using System.Diagnostics;
using System.Runtime.InteropServices;
using bikamovie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace bikamovie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly FilmDbContext ctx;


        public HomeController(ILogger<HomeController> logger, FilmDbContext ctx)
        {
            _logger = logger;
            this.ctx = ctx;
        }
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Index()
        {
            var seciliIdler = new List<int> { 5, 6, 8, 11, 26 };
            var filmler = ctx.Filmler
                .Where(f => seciliIdler.Contains(f.Id))
                .ToList();
            return View(filmler);
        }

        public IActionResult Privacy()
        {
            var data = ctx.Filmler.FirstOrDefault();
            Debug.WriteLine(data?.ToString());
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
