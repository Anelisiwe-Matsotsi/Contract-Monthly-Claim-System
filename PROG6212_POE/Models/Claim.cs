using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{
    public class Claim
    {
            [Key]
            public int ClaimId { get; set; }

            [Required(ErrorMessage = "Lecturer ID is required.")]
            public int LecturerId { get; set; }

            [Required(ErrorMessage = "Claim Date is required.")]
            [DataType(DataType.Date)]
            public DateTime SubmissionDate { get; set; }

            [Required(ErrorMessage = "Hours worked are required.")]
            [Range(1, 200, ErrorMessage = "Hours must be between 1 and 200.")]
            public int TotalHoursWorked { get; set; }

            [Required(ErrorMessage = "Hourly rate is required.")]
            [Range(100, 1000, ErrorMessage = "Hourly rate must be between R100 and R1000.")]
            public decimal HourlyRate { get; set; }


            public decimal TotalAmount => TotalHoursWorked * HourlyRate;

            [Required(ErrorMessage = "Claim status is required.")]
            [MaxLength(50)]
            public string ClaimStatus { get; set; } = "Pending";
        }
    }