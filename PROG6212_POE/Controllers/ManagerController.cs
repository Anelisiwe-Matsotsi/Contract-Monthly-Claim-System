using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_POE.Models;

namespace PROG6212_POE.Controllers
{ /*Troelsen and Japikse, 2021*/
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context; // Database context
        public ManagerController(ApplicationDbContext context)
        {

            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var claims =await _context.Claim
                .Include(c => c.Lecturer)
                .Where(c => c.ClaimStatus == "Approved by Coordinator")
                .ToListAsync();
            return View(claims);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claim.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }
            claim.ClaimStatus = "Approved by Manager";
            _context.Claim.Update(claim);
            await _context.SaveChangesAsync();

            _context.Approval.Add(new Approval
            {
                ClaimId = id,
                Decision = "Approved",
                Role = "Manager",
                ApprovalDate = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claim.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }
            claim.ClaimStatus = "Rejected by Manager";
            _context.Claim.Update(claim);
            await _context.SaveChangesAsync();

            _context.Approval.Add(new Approval
            {
                ClaimId = id,
                Decision = "Rejected",
                Role = "Manager",
                ApprovalDate = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public IActionResult DownloadDocument(int documentId)
        {
            var doc = _context.SupportingDocument.Find(documentId);
            if (doc == null)
            {
                return NotFound();
            }
            var encryptedBytes = System.IO.File.ReadAllBytes(doc.FilePath);
            var decryptedBytes = EncryptionHelper.DecryptFile(encryptedBytes);
            return File(decryptedBytes, "application/octet-stream", doc.FileName);
        }
    }
}
/*Reference List:
 Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */