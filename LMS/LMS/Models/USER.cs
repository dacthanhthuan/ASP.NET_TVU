using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class USER
    {
        [Required]
        public int Id { get; set; }
        [Required]

        public string Username { get; set; }
        [Required]

        public string Password { get; set; }

        public string Fullname { get; set; }
        public string Phone { get; set; }


    }
}
