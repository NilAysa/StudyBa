using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBaProject.Data;
using StudyBaProject.Models;
using System.Diagnostics;
using System.Linq;

namespace StudyBaProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Search(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ViewBag.Message = "Please enter a username.";
                return View();
            }

            var tutors = _context.Users
                .Where(u => u.Username.Contains(username) && u.Role == "Tutor")
                .ToList();

            if (tutors.Count == 0)
            {
                ViewBag.Message = "No tutors found.";
            }

            return View(tutors);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
