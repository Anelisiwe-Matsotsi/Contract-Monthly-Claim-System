using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{
    public class Approval
    {
        [Key]
        public int ApprovalId { get; set; }
        public int ClaimId { get; set; }
        public string Decision { get; set; }
        public string Role { get; set; }
    
        public DateTime ApprovalDate { get; set; } = DateTime.Now;
    }
}
