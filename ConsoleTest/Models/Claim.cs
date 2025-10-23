using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Models
{
    /*
    Troelson and Japiske, 2021*/
   public class Claim
    {

            public int ClaimId { get; set; }
            public int LecturerId { get; set; }
            public string Month { get; set; }
            public int TotalHoursWorked { get; set; }
            public decimal HourlyRate { get; set; }
            public decimal TotalAmount { get; set; }
            public string ClaimStatus { get; set; }
            public DateTime SubmissionDate { get; set; }
            public string Notes { get; set; }
            public string DocumentFileName { get; set; }
        }
    }
/*Reference List:
 Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */