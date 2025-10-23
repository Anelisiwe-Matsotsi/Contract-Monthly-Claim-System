using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{/*Troelsen and Japikse, 2021*/
    public class Approval
    {
        [Key]
        public int ApprovalId { get; set; }
        public int ClaimId { get; set; }
        public string Decision { get; set; }
        public string Role { get; set; }
    
        public DateTime ApprovalDate { get; set; } = DateTime.Now;
    }
}
/*Reference List:
 Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */