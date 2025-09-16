using Microsoft.AspNetCore.Mvc;

namespace PROG6212_POE.Controllers
{
    public class LecturerController : Controller
    {
        public IActionResult Index()
        {
            return View(); 
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
