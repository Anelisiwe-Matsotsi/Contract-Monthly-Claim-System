using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{ /*Troelsen and Japikse, 2021*/
    public class Lecturer
    {
        [Key]

        public int LecturerId { get; set; }
        public int AccountId { get; set; }

        public string? FullName { get; set; }

        
        public string? Department { get; set; }


        public string? Email { get; set; }
 
        public  string? Password { get; set; }
     
        public decimal HourlyRate { get; set; }
        public decimal TotalHoursWorked { get; set; }
        public decimal TotalAmount { get; set; }

        public List<Claim> Claim { get; set; } = new();

        public Account? Account { get; set; }
    }
}
/*Reference List:
 Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */