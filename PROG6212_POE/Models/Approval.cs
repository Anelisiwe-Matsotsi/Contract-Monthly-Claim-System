namespace PROG6212_POE.Models
{
    public class Approval
    {
        public int ApprovalId { get; set; }
        public string Decision { get; set; }
        public int ClaimId { get; set; }
        public DateTime ApprovalDate { get; set; }
    }
}
