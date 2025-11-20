using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_POE.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace PROG6212_POE.Controllers
{
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }
        /*Troelson and Japiske, 2021*/
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
            if (HttpContext.Session.GetString("AccountRole") != "HR")  /*Troelson and Japiske, 2021*/
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

                /*Troelson and Japiske, 2021*/
                if (account.Role == "Lecturer")
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
            if (HttpContext.Session.GetString("AccountRole") != "HR")
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
        public async Task<IActionResult> Edit(Account account, string fullName, string email, string department, decimal hourlyRate)
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




        /*Troelson and Japiske, 2021*/

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
            var averageHoursWorked = approvedClaims.Any() ? (decimal) approvedClaims.Average(c => c.TotalHoursWorked) : 0;

            //Monnthly data
            /*Microsoft, 2025*/
            var monthlyData = approvedClaims
                .GroupBy(c => c.Month)
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
                .OrderByDescending(x => x.TotalApprovedAmount)
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

        public async Task<IActionResult> ViewInvoice(int claimId)
        {
            if (HttpContext.Session.GetString("AccountRole") != "HR")
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

        public async Task<IActionResult> LecturerReport(int lecturerId)
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

        [HttpPost]
        public async Task<IActionResult>DownloadPdfReport()
        {
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
                return RedirectToAction("Index", "Account");
            }
            try
            {
                var reportData = await GetReportData();
                var pdfContent = GeneratePdfReportContent(reportData);
                var bytes = Encoding.UTF8.GetBytes(pdfContent);

                var fileName = "HR_Report.pdf";
                return File(bytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error generating PDF report: " + ex.Message;
                return RedirectToAction("GenerateReport");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DownloadInvoicePdf(int claimId)
        {
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
                return RedirectToAction("Index", "Account");
            }

            var claim = await _context.Claim
                .Include(c => c.Lecturer)
                .FirstOrDefaultAsync(c => c.ClaimId == claimId);

            if (claim == null)
            {
                TempData["Message"] = "Claim not found.";
                return RedirectToAction("GenerateReport");
            }

            try
            {
                var invoiceContent = GeneratePdfInvoiceContent(claim);
                var bytes = Encoding.UTF8.GetBytes(invoiceContent);

                var fileName = $"Invoice_{claim.Lecturer?.FullName}_{claim.Month}.pdf";
                return File(bytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error generating invoice PDF: " + ex.Message;
                return RedirectToAction("GenerateReport");
            }
        }
        /* Microsoft, 2025 */
        private string GeneratePdfReportContent(ReportData data)
        {
            using var writer = new StringWriter();

            writer.WriteLine("HR CLAIMS REPORT");
            writer.WriteLine("================");
            writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
            writer.WriteLine();

            writer.WriteLine("SUMMARY STATISTICS");
            writer.WriteLine("------------------");
            writer.WriteLine($"Total Claims: {data.AllClaims.Count}");
            writer.WriteLine($"Approved Claims: {data.ApprovedClaims.Count}");
            writer.WriteLine($"Pending Claims: {data.PendingClaims.Count}");
            writer.WriteLine($"Rejected Claims: {data.RejectedClaims.Count}");
            writer.WriteLine($"Total Approved Amount: R{data.TotalApprovedAmount:F2}");
            writer.WriteLine($"Average Hours Worked: {data.AverageHoursWorked:F1}");
            writer.WriteLine();

            writer.WriteLine("MONTHLY BREAKDOWN");
            writer.WriteLine("-----------------");
            foreach (var month in data.MonthlyData)
            {
                var average = month.TotalClaims > 0 ? month.TotalApprovedAmount / month.TotalClaims : 0;
                writer.WriteLine($"{month.Month}: {month.TotalClaims} claims, R{month.TotalApprovedAmount:F2} (Avg: R{average:F2})");
            }
            writer.WriteLine();

            writer.WriteLine("TOP LECTURERS BY EARNINGS");
            writer.WriteLine("-------------------------");
            foreach (var lecturer in data.TopLecturers)
            {
                var average = lecturer.TotalClaims > 0 ? lecturer.TotalApprovedAmount / lecturer.TotalClaims : 0;
                writer.WriteLine($"{lecturer.LecturerName} ({lecturer.Department}): R{lecturer.TotalApprovedAmount:F2} from {lecturer.TotalClaims} claims (Avg: R{average:F2})");
            }
            writer.WriteLine();

            writer.WriteLine("APPROVED CLAIMS DETAIL");
            writer.WriteLine("----------------------");
            foreach (var claim in data.ApprovedClaims.Take(20))
            {
                writer.WriteLine($"{claim.Lecturer?.FullName} - {claim.Month}: {claim.TotalHoursWorked} hours @ R{claim.HourlyRate} = R{claim.TotalAmount:F2}");
            }

            return writer.ToString();
        }

        /* Microsoft, 2025 */
        private string GeneratePdfInvoiceContent(Claim claim)
        {
            using var writer = new StringWriter();

            writer.WriteLine("INVOICE");
            writer.WriteLine("=======");
            writer.WriteLine();
            writer.WriteLine($"Invoice Date: {DateTime.Now:yyyy-MM-dd}");
            writer.WriteLine($"Invoice #: CLAIM-{claim.ClaimId}");
            writer.WriteLine($"Claim Month: {claim.Month}");
            writer.WriteLine();

            writer.WriteLine("LECTURER INFORMATION");
            writer.WriteLine("-------------------");
            writer.WriteLine($"Name: {claim.Lecturer?.FullName}");
            writer.WriteLine($"Department: {claim.Lecturer?.Department}");
            writer.WriteLine($"Email: {claim.Lecturer?.Email}");
            writer.WriteLine();

            writer.WriteLine("CLAIM DETAILS");
            writer.WriteLine("-------------");
            writer.WriteLine($"Hours Worked: {claim.TotalHoursWorked}");
            writer.WriteLine($"Hourly Rate: R{claim.HourlyRate:F2}/hour");
            writer.WriteLine($"Total Amount: R{claim.TotalAmount:F2}");
            writer.WriteLine();
            writer.WriteLine($"Submission Date: {claim.SubmissionDate:yyyy-MM-dd}");
            writer.WriteLine($"Status: {claim.ClaimStatus}");
            writer.WriteLine();

            if (!string.IsNullOrEmpty(claim.Notes))
            {
                writer.WriteLine($"Notes: {claim.Notes}");
                writer.WriteLine();
            }

            writer.WriteLine("APPROVAL INFORMATION");
            writer.WriteLine("-------------------");
            writer.WriteLine("This claim has been approved through the system workflow.");
            writer.WriteLine();
            writer.WriteLine("Generated by HR Department");
            writer.WriteLine("University Claims System");

            return writer.ToString();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Account");
        }
    }
}


/*Reference List:
Microsoft. 2025. Write C# LINQ queries to query data. [online]. Available at: <https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries>.[Accessed 19 November 2025].
Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */
