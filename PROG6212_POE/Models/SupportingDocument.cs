using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{
    public class SupportingDocument
    {
        [Key]
        [Required]
        public int DocumentId { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public string? FileName { get; set; }
        [Required]
        public string? FilePath { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public Claim? Claim { get; set; }
    }
}
