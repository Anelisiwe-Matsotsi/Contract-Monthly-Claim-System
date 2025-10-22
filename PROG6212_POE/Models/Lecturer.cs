using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{
    public class Lecturer
    {
        [Key]
        [Required]
        public int LecturerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public  string Password { get; set; }
        [Required]
        public decimal HourlyRate { get; set; }
        public decimal TotalHoursWorked { get; set; }
        public decimal TotalAmount { get; set; }


    }
}
