using System.ComponentModel.DataAnnotations;

namespace PROG6212_POE.Models
{
    public class Lecturer
    {
        [Key]

        public int LecturerId { get; set; }

 
        public string Name { get; set; }

        
        public string Department { get; set; }


        public string Email { get; set; }
 
        public  string Password { get; set; }
     
        public decimal HourlyRate { get; set; }
        public decimal TotalHoursWorked { get; set; }
        public decimal TotalAmount { get; set; }

        public List<Claim> Claim { get; set; } = new();


    }
}
