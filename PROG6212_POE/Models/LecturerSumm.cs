namespace PROG6212_POE.Models
{
    public class LecturerSumm
    {
        public string LecturerName { get; set; } = string.Empty;
        public string  Department { get; set; } = string.Empty;
        public int TotalClaims { get; set; }
        public decimal TotalApprovedAmount { get; set; }
    }
}