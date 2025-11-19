using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{/*Troelsen and Japikse, 2021*/
    public class SupportingDocument
    {
        [Key]
        [Required]
        public int DocumentId { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [Required]
        public string? FileName { get; set; }
        [Required]
        public string? FilePath { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public Claim? Claim { get; set; }
    }
}
/*Reference List:
 Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */