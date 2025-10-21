using Microsoft.AspNetCore.Mvc;
using PROG6212_POE.Models;

namespace PROG6212_POE.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context; // Database context
        public ManagerController(ApplicationDbContext context)
        {

            _context = context;
        }
        public IActionResult Index()
        {
            var finalApprove = _context.Claim.Where(c => c.ClaimStatus == "Approved by Coordinator").OrderBy(c => c.SubmissionDate).ToList();   
            return View(finalApprove);
        }

        public IActionResult Approve(int id)
        {
            var claim = _context.Claim.Find(id);
            if (claim != null)
            {
                claim.ClaimStatus = "Approved by Manager";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Reject(int id)
        {
            var claim = _context.Claim.Find(id);
            if (claim != null)
            {
                claim.ClaimStatus = "Rejected by Manager";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
