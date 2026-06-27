using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class WEEKLY_MENU
    {
        [Key]
        [Required]
        public int ID { get; set; }
        public string DAY_NAME { get; set; }
        public string BREAKFAST { get; set; }
        public string LUNCH { get; set; }
        public string DINNER { get; set; }
        public DateTime CREATED_DATE { get; set; }
    }
}
