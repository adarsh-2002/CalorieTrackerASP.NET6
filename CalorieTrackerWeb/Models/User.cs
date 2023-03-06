using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalorieTrackerWeb.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Passwords do not match!")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public byte[] salt { get; set; }
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        [Range(0,250)]
        [Display(Name="Height (in cms)")]
        public int Height { get; set; }
        [Range(0, 250)]
        [Display(Name ="Weight (in kgs)")]
        public int Weight { get; set; }
        [Range(0, 3000)]
        [Display(Name ="Required Calories (in Kcal)")]
        public int RequiredCalories { get; set; }
        public virtual List<FoodEntry> FoodEntries { get; set; }
        public virtual List<Workout> Workouts { get; set; }
        public User() { }
        public User(string name, string email, string password, DateTime dob, int height, int weight, int requiredCalories)
        {
            Name = name;
            Email = email;
            Password = password;
            Dob = dob;
            Height = height;
            Weight = weight;
            RequiredCalories = requiredCalories;
        }
    }
}
