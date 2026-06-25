using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class FOOD
    {
        [Key]
        [Required]
        public int FOOD_ID { get; set; }
        public string? FOOD_NAME { get; set; }
        public string? IMAGE_URL { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? COOK_TIME { get; set; }
        public int IS_ACTIVE { get; set; }
    }
}
