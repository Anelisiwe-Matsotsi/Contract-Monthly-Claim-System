using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG6212_POE.Models
{ /*Troelsen and Japikse, 2021*/
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Lecturer ID is required.")]
        public int LecturerId { get; set; }


        [Required(ErrorMessage = "Month is required.")]
        public string Month { get; set; }

        [Required(ErrorMessage = "Claim Date is required.")]
        [DataType(DataType.Date)]
        public DateTime SubmissionDate { get; set; }

        [Required(ErrorMessage = "Hours worked are required.")]
        [Range(1, 200, ErrorMessage = "Hours must be between 1 and 200.")]
        public int TotalHoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly rate is required.")]
        [Range(100, 1000, ErrorMessage = "Hourly rate must be between R100 and R1000.")]
        public decimal HourlyRate { get; set; }

        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Claim status is required.")]
         public string ClaimStatus { get; set; } = "Pending";
        public Lecturer? Lecturer { get; set; }
    

        public string Notes { get; set; }

        public string DocumentFileName { get; set; }
    }
}
/*Reference List:
 Troelsen,A. and Japikse, P. 2021.Pro C# 9 with .NET 5:Foundational Principles and Practices in Programming. 10th ed.Apress.
 */