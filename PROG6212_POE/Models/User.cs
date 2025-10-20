using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{
    public class User
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // e.g., "Lecturer", "Coordinator", "Manager"
    }
}
 