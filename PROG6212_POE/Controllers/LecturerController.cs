using Microsoft.AspNetCore.Mvc;
using PROG6212_POE.Services;
using System.Security.Claims;

namespace PROG6212_POE.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ClaimService _claimService;

        public LecturerController()
        {
            _claimService = new ClaimService();
        }

        //Submit new claim
        [HttpGet]
        public IActionResult Submit()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(Claim claim)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest("Invalid claim data. Please fill in all required fields correctly.");
                }

            }
        }

        public IActionResult Index()
        {
            return View(); 
        }
        [HttpPost]
        public IActionResult Create(IFormFile uploadFile)
        {

            return View();
        }
    }
}
