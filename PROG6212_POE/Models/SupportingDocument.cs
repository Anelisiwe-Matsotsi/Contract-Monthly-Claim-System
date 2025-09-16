namespace PROG6212_POE.Models
{
    public class SupportingDocument
    {
        public int DocumentId { get; set; }
        public int ClaimId { get; set; }
        public int UserId { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
    }
}
