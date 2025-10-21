using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_POE.Models;
using System.IO;

namespace PROG6212_POE.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly ApplicationDbContext _context; // Database context
        public CoordinatorController(ApplicationDbContext context)
        {
            
            _context = context;
        }
        
        public async Task <IActionResult> Index()
        {
            var pendingClaims = await _context.Claim
                .Include(c => c.SupportingDocument)
                .Where(c => c.ClaimStatus == "Pending")
                .ToListAsync();
            return View(pendingClaims);
        }
        [HttpPost]
        public async Task<IActionResult> Approve(int claimId, string decision)
        {
            var claim = await _context.Claim.FindAsync(claimId);
            if(claim == null)
            {
                return NotFound();
            }
            claim.ClaimStatus = decision == "Approve" ? "Approved by Coordinator" : "Rejected";
            _context.Claim.Update(claim);
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
