using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PROG6212_POE.Models;
using PROG6212_POE.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;

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
        public IActionResult Index()
        {
            var claims = _context.Claim.ToList();
            return View(claims); 
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Claim claim, IFormFile uploadFile)
        {
            if (ModelState.IsValid)
            {
                claim.TotalAmount = claim.HourlyRate * claim.TotalHoursWorked;
                claim.ClaimStatus = "Pending";
                claim.SubmissionDate = DateTime.Now;

                //This is to handle file upload
                if (uploadFile != null && uploadFile.Length > 0) /*C# Corner, 2024*/
                {
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string fileName = Path.GetFileName(uploadFile.FileName);
                    string fullPath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        uploadFile.CopyTo(stream);
                    }

                    claim.UploadedFilePath = "/uploads/" + fileName;
                }

                _context.Claim.Add(claim);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(claim);
        }

        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return BadRequest("Invalid claim ID.");
            }
            var claim = _context.Claim.FirstOrDefault(c => c.ClaimId == id);
            if(claim == null)
            {
                return NotFound("Claim not found.");
            }
            return View(claim);
        }

        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return BadRequest("Invalid claim ID.");
            }
            var claim = _context.Claim.FirstOrDefault(c => c.ClaimId == id);
            if (claim == null)
            {
                return NotFound("Claim not found.");
            }
            return View(claim);
        }
        public IActionResult ConfirmDelete(int id)
        {
            var claim = _context.Claim.FirstOrDefault(c => c.ClaimId == id);
            if (claim== null)
            {
                return NotFound();
            }
            _context.Claim.Remove(claim);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
/*Reference List:
C# Corner. 2024. Simplifying File Uploads with .NET Core 7 Minimal APIs. [online]. Available at: <https://www.c-sharpcorner.com/article/simplifying-file-uploads-with-net-core-7-minimal-apis/>. [Accessed 19 October 2025].

 */