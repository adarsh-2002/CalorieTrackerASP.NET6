using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CalorieTrackerWeb.Models;

namespace CalorieTrackerWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly CalorieTrackerContext _context;
        private readonly ISession session;

        public UsersController(CalorieTrackerContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            session = httpContextAccessor.HttpContext.Session;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if(session.GetString("Admin") == "True")
                return View(await _context.Users.ToListAsync());
            else
                return Unauthorized();
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details()
        {
            if(session.GetInt32("ID") != null)
            {
                int id = (int)session.GetInt32("ID");
                var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            TempData["error"] = "An error occurred";
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit()
        {
            if (session.GetInt32("ID") == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(session.GetInt32("ID"));
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,Email,Password,Dob,Gender,Height,Weight,RequiredCalories")] User user)
        {
            if (session.GetInt32("ID") != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details");
            }
            TempData["success"] = "Details updated successfully";
            return RedirectToAction("Details");
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete()
        {
            if (session.GetInt32("id") == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == session.GetInt32("ID"));
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'CalorieTrackerContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(session.GetInt32("ID"));
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            session.Clear();
            TempData["success"] = "Your account has been deleted and you have been logged out!";
            return RedirectToAction("Index", "Home");
        }

        private bool UserExists(int id)
        {
          return _context.Users.Any(e => e.Id == id);
        }
    }
}
