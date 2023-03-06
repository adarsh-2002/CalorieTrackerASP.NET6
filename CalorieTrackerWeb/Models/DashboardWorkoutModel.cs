using System.ComponentModel.DataAnnotations;

namespace CalorieTrackerWeb.Models
{
    public class DashboardWorkoutModel
    {
        [Key]
        public DateTime Date { get; set; }
        [Display(Name = "Total Calories Burned")]
        public float TotalWCals { get; set; }
    }
}
