using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PROG6212_POE.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Claim = PROG6212_POE.Models.Claim;
using AspNetCoreGeneratedDocument;

namespace PROG6212_POE.Controllers
{ /*Troelsen and Japikse, 2021*/
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context; // database context
        private readonly IWebHostEnvironment _webHostEnvironment; // used to access wwwroot and  file

        public LecturerController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task <IActionResult> Index()
        {

            var claims = await _context.Claim
                .Include(c => c.Lecturer)
                .ToListAsync();

            return View(claims);
        }

        [HttpGet]
        // Create a new claim
        public IActionResult Create()
        {
            ViewBag.Lecturer = _context.Lecturer.ToList();
            return View();
        }


        [HttpPost]
        [RequestSizeLimit(500000)] /*C# Corner, 2024*/
        public async Task<IActionResult> Create(IFormFile uploadFile, int TotalHoursWorked, decimal HourlyRate, string ClaimStatus, string Month, string Notes, int LecturerId)
        {

            /*Troelsen and Japikse, 2021*/
            var lecturer = await _context.Lecturer.FindAsync(LecturerId);
            if(lecturer == null)
            {
                ViewBag.Error = "Lecturer not found.";
                return View();
            }

            // validate hours worked (max is 180 hours per month)
            if (TotalHoursWorked > 180)
            {
                ModelState.AddModelError("TotalHoursWorked", "Total hours worked cannot exceed 180 hours per month.");
                ViewBag.Lecturer = _context.Lecturer.ToList();
                return View();
            }

            /*Troelsen and Japikse, 2021*/
            //Handle file upload
            if (uploadFile != null && uploadFile.Length > 0)
            {
                var fileName = Path.GetFileName(uploadFile.FileName);
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

                using (var stream = new MemoryStream())
                {
                    uploadFile.CopyTo(stream);
                    var fileBytes = stream.ToArray();
                    var encryptedBytes = EncryptionHelper.EncryptFile(fileBytes);
                    System.IO.File.WriteAllBytes(path, encryptedBytes);
                }
            if(uploadFile.Length > 500000)
                {
                    TempData["Message"] = "File exceeds limit";
                    return View();
                }
                decimal totalAmount = TotalHoursWorked * HourlyRate; 

                var claims = new Claim
                {
                    LecturerId = LecturerId,
                    Month = Month,
                    SubmissionDate = DateTime.Now,
                    TotalHoursWorked = TotalHoursWorked,
                    HourlyRate = lecturer.HourlyRate,
                    TotalAmount = totalAmount,
                    ClaimStatus = "Pending",
                    Notes = Notes,
                    DocumentFileName = fileName

                };
                _context.Claim.Add(claims);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

                TempData["Message"] = "File exceeds limit";
                ViewBag.Lecturer = _context.Lecturer.ToList();
                return View();

        }

        public ActionResult Download(string fileName)
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            var encryptedBytes = System.IO.File.ReadAllBytes(path);
            var decryptedBytes = EncryptionHelper.DecryptFile(encryptedBytes);
            return File(decryptedBytes, "application/octet-stream", fileName);
        }
    }
}
/*Reference List:
C# Corner. 2024. Simplifying File Uploads with .NET Core 7 Minimal APIs. [online]. Available at: <https://www.c-sharpcorner.com/article/simplifying-file-uploads-with-net-core-7-minimal-apis/>. [Accessed 19 October 2025].
Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress. 
*/