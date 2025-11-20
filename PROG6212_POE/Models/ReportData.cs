namespace PROG6212_POE.Models
{
    public class ReportData
    {
        public List<Claim> AllClaims { get; set; } = new();
        public List<Claim> ApprovedClaims { get; set; } = new();
        public List<Claim> PendingClaims { get; set; } = new();
        public List<Claim> RejectedClaims { get; set; } = new();
        public List<Lecturer> Lecturer { get; set; } = new();

        public decimal TotalApprovedAmount { get; set; }
        public decimal AverageHoursWorked { get; set; }
        public List<MonthlySumm> MonthlyData { get; set; } = new();
        public List<LecturerSumm> TopLecturers { get; set; } = new();
    }
}
