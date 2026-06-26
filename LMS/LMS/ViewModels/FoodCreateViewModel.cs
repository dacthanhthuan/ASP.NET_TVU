using LMS.Models;

namespace LMS.ViewModels
{
    public class FoodCreateViewModel
    {
        public FOOD Food { get; set; } = new();

        public List<CATEGORY> Categories { get; set; } = new();

        public List<FOOD> Foods { get; set; } = new();
    }
}