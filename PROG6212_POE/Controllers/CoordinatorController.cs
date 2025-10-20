using Microsoft.AspNetCore.Mvc;
using PROG6212_POE.Models;

namespace PROG6212_POE.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly ApplicationDbContext _context; // Database context
        public CoordinatorController(ApplicationDbContext context)
        {
            
            _context = context;
        }
        public IActionResult Index()
        {
            var pending = _context.Claim.Where(c => c.ClaimStatus == "Pending").OrderBy(c => c.SubmissionDate).ToList();
            return View(pending); 
        }

        public IActionResult Approve(int id)
        {
            var claim = _context.Claim.Find(id);
            if(claim != null)
            {
                claim.ClaimStatus = "Approved by Coordinator";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Reject(int id)
        {
            var claim = _context.Claim.Find(id);
            if (claim != null)
            {
                claim.ClaimStatus = "Rejected by Coordinator";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View(); //Display the claim submission form
        }
    }
}
