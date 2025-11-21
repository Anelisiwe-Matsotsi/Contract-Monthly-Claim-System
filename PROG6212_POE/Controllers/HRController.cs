using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_POE.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace PROG6212_POE.Controllers
{
    //Responsible for HR functionalities which includes managing accounts and generating reports
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }
        /*Troelson and Japiske, 2021*/
        //HR only access is enforced using Sessions
        public async Task<IActionResult> Index()
        {
            //This is to check if the user is HR
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
                return RedirectToAction("Index", "Account");
            }
            //retrieves all accounts with their linked Lecturer details
            var users = await _context.Account
                .Include(a => a.Lecturer)
                .ToListAsync();
            return View(users);
        }

        //Automatically creates a linked Lecturer when the Role is set to Lecturer
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
        //handles creation of both Account and linked Lecturer
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
                //if the created account is a Lecturer, create the linked Lecturer record
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
        //displays the edit form for the selected account
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
                return RedirectToAction("Index", "Account");
            }

            //fetches the account along with linked Lecturer details
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
        /*Troelson and Japiske, 2021*/
        //handles updating both Account and linked Lecturer details
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
                //Updates linked Lecturer
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

        //Generates the HR report view, which initiates data retrieval for the report
        public async Task<IActionResult> GenerateReport()
        {
            if (HttpContext.Session.GetString("AccountRole") != "HR")
            {
                return RedirectToAction("Index", "Account");
            }
            //fetches all necessary data for the report
            var reportData = await GetReportData();
            return View(reportData);
        }




        /*Troelson and Japiske, 2021*/
        /*Microsoft, 2025a*/
        //constructs the report data by querying and processing claims and lecturers
        public async Task<ReportData> GetReportData()
        {
            var allClaims = await _context.Claim
                .Include(c => c.Lecturer)
                .ToListAsync();

            var allLecturers = await _context.Lecturer.ToListAsync();

            //Status filtering using LINQ
            var approvedClaims = allClaims
                .Where(c => c.ClaimStatus == "Approved by Manager")
                .ToList();
            var pendingClaims = allClaims
                .Where(c => c.ClaimStatus.Contains("Pending"))
                .ToList();
            var rejectedClaims = allClaims
                .Where(c => c.ClaimStatus.Contains("Rejected"))
                .ToList();
            //Summary statistics calculations
            var totalApprovedAmount = approvedClaims.Sum(c => c.TotalAmount);
            var averageHoursWorked = approvedClaims.Any() ? (decimal) approvedClaims.Average(c => c.TotalHoursWorked) : 0;

            //Monnthly data summary using LINQ group by
            /*Microsoft, 2025b*/
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

            //Top 5 lecturers by approved amount
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

        //displays details of a specific claim invoice
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

        //generates a report filtered by a specific lecturer
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

        //generates and downloads the full HR report as a PDF
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

        //generates and downloads a specific claim invoice as a PDF
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
        /* Microsoft, 2025b */
        //converts the report data into a formatted string for PDF generation using StringWriter
        private string GeneratePdfReportContent(ReportData data)
        {
            /*Stackoverflow, 2012*/
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
                var average = month.TotalClaims > 0 ? month.TotalApprovedAmount / month.TotalClaims : 0; /*Programmershelp, 2025*/
                writer.WriteLine($"{month.Month}: {month.TotalClaims} claims, R{month.TotalApprovedAmount:F2} (Avg: R{average:F2})");
            }
            writer.WriteLine();

            writer.WriteLine("TOP LECTURERS BY EARNINGS");
            writer.WriteLine("-------------------------");
            foreach (var lecturer in data.TopLecturers)
            {
                //Calculates average approved amount per claim for each lecturer
                var average = lecturer.TotalClaims > 0 ? lecturer.TotalApprovedAmount / lecturer.TotalClaims : 0;
                writer.WriteLine($"{lecturer.LecturerName} ({lecturer.Department}): R{lecturer.TotalApprovedAmount:F2} from {lecturer.TotalClaims} claims (Avg: R{average:F2})");
            }
            writer.WriteLine();

            writer.WriteLine("APPROVED CLAIMS DETAIL");
            writer.WriteLine("----------------------");
            //loops through the first 20 approved claims in the collection and writes each claim detail
            foreach (var claim in data.ApprovedClaims.Take(20))/*Troelson and Japiske, 2021*/ 
            {
                writer.WriteLine($"{claim.Lecturer?.FullName} - {claim.Month}: {claim.TotalHoursWorked} hours @ R{claim.HourlyRate} = R{claim.TotalAmount:F2}");
            }

            return writer.ToString();
        }

        /* Microsoft, 2025b*/
        //converts a specific claim into a formatted string for PDF invoice generation using StringWriter
        private string GeneratePdfInvoiceContent(Claim claim)
        {
            /*Stackoverflow, 2012*/
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
        //clears the session and logs out the user
        /*Troelson and Japiske, 2021*/
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Account");
        }
    }
}


/*Reference List:
Microsoft. 2025a. StringWriter Class. [online]. Available at: <https://learn.microsoft.com/en-us/dotnet/api/system.io.stringwriter?view=net-10.0>.[Accessed 19 November 2025].
Microsoft. 2025b. Write C# LINQ queries to query data. [online]. Available at: <https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries>.[Accessed 19 November 2025].
Programmershelp. 2025. How to Fix DivideByZeroException in C# – Beginner-Friendly Guide. [online]. Available at: <https://www.programmershelp.net/tips/how-to-fix-dividebyzeroexception-in-c-beginner-friendly-guide.php>. [Accessed 19 November 2025].
Stackoverflow. 2012. Convert StringWriter to string[]. [online]. Available at: <https://stackoverflow.com/questions/10303315/convert-stringwriter-to-string>. [Accessed 19 November 2025].
Troelsen, A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */
