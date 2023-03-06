using CalorieTrackerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CalorieTrackerWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly CalorieTrackerContext _context;
        private readonly ISession session;

        public LoginController(CalorieTrackerContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            session = httpContextAccessor.HttpContext.Session;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(User u)
        {
            if(u.Email != null && u.Password != null)
            {
                var result = _context.Users.SingleOrDefault(x => x.Email == u.Email);
                if(result != null)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                               password: u.Password,
                                               salt: result.salt,
                                               prf: KeyDerivationPrf.HMACSHA256,
                                               iterationCount: 1000,
                                               numBytesRequested: 256 / 8));
                    if(hashed == result.Password)
                    {
                        session.SetString("Username", result.Name);
                        session.SetInt32("ID", result.Id);
                        if (result.Email == "admin@gmail.com")
                            session.SetString("Admin", "True");
                        TempData["success"] = "Login Successful";
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid User");
            }
            TempData["error"] = "Invalid User!";
            return View();
        }
        public IActionResult SignUp()
        {
            if (session.GetInt32("ID") != null)
            {
                session.Clear();
                TempData["warning"] = "You have been signed out!";
                return View();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(User user)
        {
            if (user != null)
            {
                if(user.Dob > DateTime.Now)
                {
                    TempData["error"] = "Invalid Date of Birth!";
                    return View();
                }
                user.salt = RandomNumberGenerator.GetBytes(128 / 8);
                user.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: user.Password,
                    salt: user.salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                try
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Account created successfully";
                    return RedirectToAction("Index");
                }
                catch(DbUpdateException e)
                {
                    Console.WriteLine(e.ToString());
                    TempData["error"] = "User with email already exists!";
                    return View();
                }
            }
            TempData["error"] = "Couldn't create account!";
            ModelState.AddModelError("","Invalid details!");
            return View();
        }
        public IActionResult SignOut()
        {
            session.Clear();
            TempData["success"] = "You have been logged out successfully!";
            return RedirectToAction("Index", "Home");
        }

    }
}
