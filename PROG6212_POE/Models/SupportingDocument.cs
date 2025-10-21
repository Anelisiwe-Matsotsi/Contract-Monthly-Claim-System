using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{
    public class SupportingDocument
    {
        [Required]
        public int DocumentId { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [Required]
        public int UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
