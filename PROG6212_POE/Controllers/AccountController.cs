using Microsoft.AspNetCore.Mvc;
using PROG6212_POE.Models;


namespace PROG6212_POE.Controllers
{
    /* Troelsen and Japikse, 2021*/
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {

            var validAccount = _context.Account.FirstOrDefault(a => a.Username == username && a.Password == password);

            if (validAccount == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View("Index");
            }

            //---SESSION CREATION---
            // If credentials are valid, create a session variable
            // to store the username.
            // This marks the user as "logged in".

            HttpContext.Session.SetString("AccountUsername", validAccount.Username);
            HttpContext.Session.SetString("AccountRole", validAccount.Role);
            HttpContext.Session.SetString("UserId", validAccount.Id.ToString());

            if (validAccount.Role == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Lecturer");
            }

        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Clear the session to log out the user
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
/*Reference List:
 Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */