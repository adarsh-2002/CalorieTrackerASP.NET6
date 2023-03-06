using CalorieTrackerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CalorieTrackerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISession session;
        private readonly CalorieTrackerContext _context;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, CalorieTrackerContext calorieTrackerContext)
        {
            _logger = logger;
            session = httpContextAccessor.HttpContext.Session;
            _context = calorieTrackerContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Dashboard()
        {
            if(session.GetInt32("ID") != null){
                //query the context to find total calories in a given day
                var feResults = (from fe in _context.FoodEntries.ToList().OrderByDescending(x=>x.Date)
                                 where fe.UserId == session.GetInt32("ID")
                                 group fe by fe.Date.Date into feGroup
                                       select new
                                       {
                                           Date = feGroup.Key,
                                           TotalCal = feGroup.Sum(x => x.Calories),
                                           TotalProteins = feGroup.Sum(x => x.Proteins),
                                           TotalCarbs = feGroup.Sum(x => x.Carbs),
                                           TotalFats = feGroup.Sum(x=> x.Fats)
                                       }).ToList();
                var wResults = (from w in _context.Workouts.ToList().OrderByDescending(x=>x.WDateTime)
                                where w.UserId == (int)session.GetInt32("ID")
                                group w by w.WDateTime.Date into wGroup
                                select new
                                {
                                    Date = wGroup.Key,
                                    TotalWCal = wGroup.Sum(x => x.Calories)
                                }).ToList();
                Console.WriteLine(feResults.Count);
                Console.WriteLine(wResults.Count);
                List<DashboardFoodModel> FoodObj = new List<DashboardFoodModel>();
                foreach (var item in feResults)
                {
                    DashboardFoodModel dfm = new DashboardFoodModel();
                    dfm.Date = item.Date;
                    dfm.TotalCal = item.TotalCal;
                    dfm.TotalPro = item.TotalProteins;
                    dfm.TotalCar = item.TotalCarbs;
                    dfm.TotalFat = item.TotalFats;
                    FoodObj.Add(dfm);
                }
                List<DashboardWorkoutModel> WorkoutObj = new List<DashboardWorkoutModel>();
                foreach (var item in wResults)
                {
                    DashboardWorkoutModel dwm = new DashboardWorkoutModel();
                    dwm.Date = item.Date;
                    dwm.TotalWCals = item.TotalWCal;
                    WorkoutObj.Add(dwm);
                }
                DashboardModel model = new DashboardModel(FoodObj, WorkoutObj);
                return View(model);
            }
            TempData["warning"] = "Please login to access the dashboard!";
            return RedirectToAction("Index","Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}