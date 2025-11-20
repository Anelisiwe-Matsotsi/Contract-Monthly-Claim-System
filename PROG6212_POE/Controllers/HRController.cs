using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_POE.Models;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PROG6212_POE.Controllers
{
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }   
        public async Task<IActionResult> Index()
        {
            //This is to check if the user is HR
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
                TempData["Message"] = "Access denied. HR only area.";
                return RedirectToAction("Index", "Account");
            }

            var users = await _context.Account
                .Include(a => a.Lecturer)
                .ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
                TempData["Message"] = "Access denied. HR only area.";
                return RedirectToAction("Index", "Account");
            }   
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Account account, string fullName, string email, string department, decimal hourlyRate)
        {
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
                TempData["Message"] = "Access denied. HR only area.";
                return RedirectToAction("Index", "Account");
            }

            if (ModelState.IsValid)
            {
                _context.Account.Add(account);
                await _context.SaveChangesAsync();

                if(account.Role == "Lecturer")
                {
                    var lecturer = new Lecturer
                    {
                        AccountId = account.Id,
                        FullName = fullName,
                        Email = email,
                        Department = department,
                        Password = account.Password,
                        HourlyRate = hourlyRate,
                        TotalHoursWorked = 0,
                        TotalAmount = 0

                    };
                    _context.Lecturer.Add(lecturer);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if(HttpContext.Session.GetString("AccountRole") != "HR")
            {
                TempData["Message"] = "Access denied. HR only area.";
                return RedirectToAction("Index", "Account");
            }
            var account = await _context.Account
                .Include(a => a.Lecturer)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Edit (Account account, string fullName, string email, string department, decimal hourlyRate)
        {
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
                TempData["Message"] = "Access denied. HR only area.";
                return RedirectToAction("Index", "Account");
            }
            if (ModelState.IsValid)
            {
                _context.Account.Update(account);
                await _context.SaveChangesAsync();
                var lecturer = await _context.Lecturer.FirstOrDefaultAsync(l => l.AccountId == account.Id);
                if (lecturer != null && account.Role == "Lecturer")
                {
                    lecturer.FullName = fullName;
                    lecturer.Email = email;
                    lecturer.Department = department;
                    lecturer.HourlyRate = hourlyRate;
                    _context.Lecturer.Update(lecturer);
                   
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        public async Task<IActionResult> GenerateReport()
        {
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
                TempData["Message"] = "Access denied. HR only area.";
                return RedirectToAction("Index", "Account");
            }

            var claims = await _context.Claim
                .Include(c => c.Lecturer)
                .Where(c => c.ClaimStatus == "Approved by Manager")
                .ToListAsync();

            return View(claims);
        }
    }
}
