using System.ComponentModel.DataAnnotations;

namespace CalorieTrackerWeb.Models
{
    public class DashboardFoodModel
    {
        [Key]
        public DateTime Date { get; set; }
        [Display(Name ="Total Calories")]
        public float TotalCal { get; set; }
        [Display(Name = "Total Proteins")]
        public float TotalPro { get; set; }
        [Display(Name = "Total Carbs")]
        public float TotalCar { get; set; }
        [Display(Name = "Total Fats")]
        public float TotalFat { get; set; }
        public DashboardFoodModel() { }
        public DashboardFoodModel(DateTime date, float totalCal, float totalPro, float totalCar, float totalFat)
        {
            Date = date;
            TotalCal = totalCal;
            TotalPro = totalPro;
            TotalCar = totalCar;
            TotalFat = totalFat;
        }
    }
}
