namespace PROG6212_POE.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public int LecturerId { get; set; }
        public string Month { get; set; }
        public decimal HourlyRate { get; set; }
        public int TotalHoursWorked { get; set; }
        public decimal TotalAmount { get; set; }
        public string ClaimStatus { get; set; } 
        public DateTime SubmissionDate { get; set; }
    }
}
