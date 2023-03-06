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
    public class WorkoutsController : Controller
    {
        private readonly CalorieTrackerContext _context;
        private readonly ISession session;

        public WorkoutsController(CalorieTrackerContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            session = httpContextAccessor.HttpContext.Session;
        }

        // GET: Workouts
        public async Task<IActionResult> Index()
        {
            if (session.GetInt32("ID") == null)
                return Unauthorized();
            int id = (int)session.GetInt32("ID");
            var calorieTrackerContext = _context.Workouts.Include(w => w.User).AsQueryable();
            calorieTrackerContext = calorieTrackerContext.Where(f => f.UserId == id).OrderByDescending(x => x.WDateTime);
            return View(await calorieTrackerContext.ToListAsync());
        }

        // GET: Workouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // GET: Workouts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,WDateTime,Name,Calories")] Workout workout)
        {
            if(session.GetInt32("ID") == null) return Unauthorized();
            if (workout != null)
            {
                if(workout.WDateTime == null)
                    workout.WDateTime = DateTime.Now;
                if(workout.WDateTime > DateTime.Now)
                {
                    TempData["error"] = "Date cannot be in the future!";
                    return View(workout);
                }
                workout.UserId = (int)session.GetInt32("ID");
                try
                {
                    _context.Add(workout);
                    await _context.SaveChangesAsync();
                }catch(Exception ex)
                {
                    TempData["error"] = "Error in adding workout";
                    return View(workout);
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error occurred while attempting to add entry!";
            return View(workout);
        }

        // GET: Workouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", workout.UserId);
            return View(workout);
        }

        // POST: Workouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,WDateTime,Name,Calories")] Workout workout)
        {
            if (id != workout.Id)
            {
                return NotFound();
            }

            if (workout != null)
            {
                if (workout.WDateTime == null)
                    workout.WDateTime = DateTime.Now;
                if (workout.WDateTime > DateTime.Now)
                {
                    TempData["error"] = "Date cannot be in the future!";
                    return View(workout);
                }
                workout.UserId = (int)session.GetInt32("ID");
                try
                {
                    _context.Update(workout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutExists(workout.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workout);
        }

        // GET: Workouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Workouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Workouts == null)
            {
                return Problem("Entity set 'CalorieTrackerContext.Workouts'  is null.");
            }
            var workout = await _context.Workouts.FindAsync(id);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutExists(int id)
        {
          return _context.Workouts.Any(e => e.Id == id);
        }
    }
}
