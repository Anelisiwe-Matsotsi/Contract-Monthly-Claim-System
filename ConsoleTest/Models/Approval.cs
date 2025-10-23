using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Models
{
    /*Troelson and Japiske, 2021*/
   public class Approval
    {
            public int ClaimId { get; set; }
            public string Decision { get; set; }
            public string Role { get; set; }
            public DateTime ApprovalDate { get; set; }
        }
    }

/*Reference List:
 Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */