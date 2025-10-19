using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{
    public class Lecturer
    {
        [Required]
        public int LecturerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNum { get; set; }

        [Required]
        public decimal HourlyRate { get; set; }

    }
}
