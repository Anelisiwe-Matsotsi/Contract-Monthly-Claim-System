using Microsoft.AspNetCore.Mvc;
using PROG6212_POE.Services;
using System.Security.Claims;

namespace PROG6212_POE.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context; // database context
        private readonly IWebHostEnvironment _webHostEnvironment; // used to access wwwroot and  file

        public LecturerController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var claims = _c
            return View(claims); 
        }
        [HttpPost]
        public IActionResult Create(IFormFile uploadFile)
        {

            return View();
        }
    }
}
