using Microsoft.AspNetCore.Mvc;

namespace PROG6212_POE.Controllers
{
    public class CoordinatorController : Controller
    {
        public IActionResult Index()
        {
            return View(); //Display list of claims 
        }

        public IActionResult Create()
        {
            return View(); //Display the claim submission form
        }
    }
}
