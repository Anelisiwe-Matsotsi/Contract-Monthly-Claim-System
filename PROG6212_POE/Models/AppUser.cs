using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // e.g., "Lecturer", "Coordinator", "Manager"
    }
}
 