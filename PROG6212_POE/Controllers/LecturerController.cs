using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PROG6212_POE.Models;
using PROG6212_POE.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace PROG6212_POE.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context; // database context
        private readonly IWebHostEnvironment _webHostEnvironment; // used to access wwwroot and  file

        public LecturerController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(int lecturerId)
        {
            var claims = await _context.Claim
                .Include(c => c.SupportingDocument)
                .Where(c => c.LecturerId == lecturerId)
                .ToListAsync();
            return View(claims);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RequestSizeLimit(500000)] /*C# Corner, 2024*/
        public async Task<IActionResult> Create(Models.Claim claim, IFormFile uploadFile)
        {
            if (ModelState.IsValid)
            {
                claim.TotalAmount = claim.HourlyRate * claim.TotalHoursWorked;
                _context.Claim.Add(claim);
                await _context.SaveChangesAsync();

                if (uploadFile != null && uploadFile.Length > 0)
                {
                    string fileName = Path.GetFileName(uploadFile.FileName);
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

                    using (var stream = new MemoryStream())
                    {
                        await uploadFile.CopyToAsync(stream);
                        var encryptedBytes = EncryptionHelper.EncryptFile(stream.ToArray());
                        System.IO.File.WriteAllBytes(path, encryptedBytes);
                    }

                    var doc = new SupportingDocument
                    {
                        ClaimId = claim.ClaimId,
                        FileName = fileName,
                        FilePath = path
                    };
                    _context.SupportingDocument.Add(doc);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index), new { lecturerId = claim.LecturerId });
            }
            return View(claim);
        }

        public IActionResult Download(string fileName)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            var encryptedBytes = System.IO.File.ReadAllBytes(path);
            var decryptedBytes = EncryptionHelper.DecryptFile(encryptedBytes);
            return File(decryptedBytes, "application/octet-stream", fileName);
        }
    }
}
/*Reference List:
C# Corner. 2024. Simplifying File Uploads with .NET Core 7 Minimal APIs. [online]. Available at: <https://www.c-sharpcorner.com/article/simplifying-file-uploads-with-net-core-7-minimal-apis/>. [Accessed 19 October 2025].
 */