using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }

        [Required]
        public int LecturerId { get; set; }
        [Required]
        public string LecturerName { get; set; }
        public string Month { get; set; }
        public decimal HourlyRate { get; set; }
        [Required]
        [Range(1, 1000)]
        public int TotalHoursWorked { get; set; }
        public decimal TotalAmount { get; set; }
        public string ClaimStatus { get; set; } = "Pending";
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
    }
}
