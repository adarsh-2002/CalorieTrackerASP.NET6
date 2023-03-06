namespace CalorieTrackerWeb.Models
{
    public class DashboardModel
    {
        public List<DashboardFoodModel> Foods { get; set; }
        public List<DashboardWorkoutModel> Workouts { get; set; }
        public DashboardModel() { }
        public DashboardModel(List<DashboardFoodModel> foods, List<DashboardWorkoutModel> workouts)
        {
            Foods = foods;
            Workouts = workouts;
        }
    }
}
