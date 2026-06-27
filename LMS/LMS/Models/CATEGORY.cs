using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class CATEGORY
    {
        [Key]
        [Required]
        public int CATEGORY_ID { get; set; }
        public string? CATEGORY_NAME { get; set; }
        public string? ICON { get; set; }
        public string? DESCRIPTION { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public int IS_ACTIVE { get; set; }
    }
}
