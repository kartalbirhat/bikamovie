using Microsoft.AspNetCore.Mvc;

namespace bikamovie.Controllers
{
    public class HakkindaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

