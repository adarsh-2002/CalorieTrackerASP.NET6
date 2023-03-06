using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalorieTrackerWeb.Models
{
    public class FoodEntry
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(minimum: 0,maximum: int.MaxValue)]
        public float Quantity { get; set; }
        public DateTime Date { get; set; }
        [Range(minimum: 0, maximum: int.MaxValue)]
        public float Calories { get; set; }
        [Range(minimum: 0, maximum: int.MaxValue)]
        public float Proteins { get; set; }
        [Range(minimum: 0, maximum: int.MaxValue)]
        public float Carbs { get; set; }
        [Range(minimum: 0, maximum: int.MaxValue)]
        public float Fats { get; set; }
        public FoodEntry() { }
        public FoodEntry(int userId, int quantity, DateTime date, int calories, int proteins, int carbs, int fats)
        {
            UserId = userId;
            Quantity = quantity;
            Date = date;
            Calories = calories;
            Proteins = proteins;
            Carbs = carbs;
            Fats = fats;
        }
    }
}
