using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{
    public class Claim
    {
        [Required]
        public int ClaimId { get; set; }

        [Required]
        public int LecturerId { get; set; }

        [Required]
        public string LecturerName { get; set; }

        [Required]
        public string Month { get; set; }

        [Required]
        public decimal HourlyRate { get; set; }
        [Required]
        [Range(1, 1000)]
        public int TotalHoursWorked { get; set; }
        public decimal TotalAmount { get; set; }

        public string Notes { get; set; }
        public string ClaimStatus { get; set; } = "Pending";
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
    }
}
