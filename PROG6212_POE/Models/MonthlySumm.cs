namespace PROG6212_POE.Models
{
    public class MonthlySumm
    {
        public string Month { get; set; } = string.Empty;
        public int TotalClaims { get; set; }
        public decimal TotalApprovedAmount { get; set; }
    }
}