using LMS.Models;

namespace LMS.ViewModels
{
    public class FoodCreateViewModel
    {
        public FOOD Food { get; set; }        // dữ liệu món ăn
        public List<CATEGORY> Categories { get; set; } // dropdown
    }
}
