using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PROG6212_POE.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        private static List<Lecturer> claims = new List<Lecturer>();


        // Create a new claim
        public IActionResult Create()
        {
            return View();
        }
        public static List<Lecturer> Claims ()
        {
            return claims;
        }

        public IActionResult Index()
        {
            return View(claims);
        }



        [HttpPost]
        [RequestSizeLimit(500000)] /*C# Corner, 2024*/
        public async Task<IActionResult> Create(Models.Claim claim, IFormFile uploadFile, int LecturerId, decimal TotalHoursWorked, decimal HourlyRate, string ClaimStatus)
        {


            //Handle file upload
            /*Troelsen and Japikse, 2021*/
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
                decimal totalAmount = TotalHoursWorked * HourlyRate; 

                claims.Add(new Lecturer
                {
                    LecturerId = LecturerId,
                    TotalHoursWorked = TotalHoursWorked,
                    HourlyRate = HourlyRate,

                });
                _context.Claim.Add(claim);
                await _context.SaveChangesAsync();

            }
            else
            {
                TempData["Message"] = "File exceeds limit";
            }
            return RedirectToAction("Index");

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