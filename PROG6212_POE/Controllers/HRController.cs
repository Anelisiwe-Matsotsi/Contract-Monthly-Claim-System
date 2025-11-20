using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PROG6212_POE.Models;
using System.Runtime.InteropServices;
using System.Security.Claims;
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
             
                return RedirectToAction("Index", "Account");
            }   
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Account account, string fullName, string email, string department, decimal hourlyRate)
        {
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
            
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
                    await _context.SaveChangesAsync();
                }
     
                return RedirectToAction("Index");
            }
            return View(account);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if(HttpContext.Session.GetString("AccountRole") != "HR")
            {
                return RedirectToAction("Index", "Account");
            }
            var account = await _context.Account
                .Include(a => a.Lecturer)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            return View("Edit", account);
        }

        [HttpPost]
        public async Task<IActionResult> Edit (Account account, string fullName, string email, string department, decimal hourlyRate)
        {
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
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
                return RedirectToAction("Index", "Account");
            }

            var reportData = await GetReportData();
            return View(reportData);
        }

        public async Task<ReportData> GetReportData()
        {
            var allClaims = await _context.Claim
                .Include(c => c.Lecturer)
                .ToListAsync();

            var allLecturers = await _context.Lecturer.ToListAsync();

            var approvedClaims = allClaims
                .Where(c => c.ClaimStatus == "Approved by Manager")
                .ToList();
            var pendingClaims = allClaims
                .Where(c => c.ClaimStatus.Contains("Pending"))
                .ToList();
            var rejectedClaims = allClaims
                .Where(c => c.ClaimStatus.Contains("Rejected"))
                .ToList();

            var totalApprovedAmount = approvedClaims.Sum(c => c.TotalAmount);
            var averageHoursWorked = approvedClaims.Any() ? approvedClaims.Average(c => c.TotalHoursWorked) : 0;

            //Monnthly data
            var monthlyData = approvedClaims
                .GroupBy(c => c.SubmissionDate.ToString("yyyy-MM"))
                .Select(g => new MonthlySumm
                {
                    Month = g.Key,
                    TotalApprovedAmount = g.Sum(c => c.TotalAmount),
                    TotalClaims = g.Count()
                })
                .OrderByDescending(m => m.Month)
                .ToList();

            var topLecturers = approvedClaims
                .Where(c => c.Lecturer != null)
                .GroupBy(c => c.LecturerId)
                .Select(g => new LecturerSumm
                {
                    LecturerName = g.First().Lecturer?.FullName,
                    Department = g.First().Lecturer?.Department,
                    TotalApprovedAmount = g.Sum(c => c.TotalAmount),
                    TotalClaims = g.Count()
                })
                .OrderDescending(x => x.TotalApprovedAmount)
                .Take(5)
                .ToList();

            return new ReportData
            {
                AllClaims = allClaims,
                ApprovedClaims = approvedClaims,
                PendingClaims = pendingClaims,
                RejectedClaims = rejectedClaims,
                Lecturer = allLecturers,
                TotalApprovedAmount = totalApprovedAmount,
                AverageHoursWorked = averageHoursWorked,
                MonthlyData = monthlyData,
                TopLecturers = topLecturers
            };
        }

        public  async Task<IActionResult> ViewInvoice(int claimId)
        {
            if(HttpContext.Session.GetString("AccountRole") != "HR")
            {
                return RedirectToAction("Index", "Account");
            }

            var claims = await _context.Claim
                .Include(c => c.Lecturer)
                .FirstOrDefaultAsync(c => c.ClaimId == claimId);

            if (claims == null)
            {
                TempData["Message"] = "Claim not found.";
                return RedirectToAction("GenerateReport");
            }

            return View(claims);
        }

        public async Task<IActionResult> LecturerReport (int lecturerId)
        {
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
                return RedirectToAction("Index", "Account");
            }
            var lecturerClaims = await _context.Claim
                .Include(c => c.Lecturer)
                .Where(c => c.LecturerId == lecturerId)
                .OrderByDescending(c => c.SubmissionDate)
                .ToListAsync();

            var lecturer = await _context.Lecturer
                .FindAsync(lecturerId);

            ViewBag.LecturerName = lecturer?.FullName;
            return View(lecturerClaims);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Account");
        }
        }
    }

