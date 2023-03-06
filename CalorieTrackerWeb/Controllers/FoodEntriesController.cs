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
    public class FoodEntriesController : Controller
    {
        private readonly CalorieTrackerContext _context;
        private readonly ISession session;

        public FoodEntriesController(CalorieTrackerContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            session = httpContextAccessor.HttpContext.Session;
        }

        // GET: FoodEntries
        public async Task<IActionResult> Index()
        {
            if (session.GetInt32("ID") == null)
                return Unauthorized();
            int id = (int)session.GetInt32("ID");
            var calorieTrackerContext = _context.FoodEntries.Include(f => f.User).AsQueryable();
            calorieTrackerContext = calorieTrackerContext.Where(f => f.UserId == id).OrderByDescending(x=>x.Date);
            return View(await calorieTrackerContext.ToListAsync());
        }

        // GET: FoodEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FoodEntries == null)
            {
                return NotFound();
            }

            var foodEntry = await _context.FoodEntries
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodEntry == null)
            {
                return NotFound();
            }

            return View(foodEntry);
        }

        // GET: FoodEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Quantity,Date,Calories,Proteins,Carbs,Fats")] FoodEntry foodEntry)
        {
            if (session.GetInt32("ID") == null)
                return Unauthorized();
            if (foodEntry !=null)
            {
                try
                {
                    foodEntry.UserId = (int)session.GetInt32("ID");
                    _context.Add(foodEntry);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Error occurred while attempting to add entry!";
                    return View(foodEntry);
                }
            }
            TempData["error"] = "Error occurred while attempting to add entry!";
            return View(foodEntry);
        }

        // GET: FoodEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FoodEntries == null)
            {
                return NotFound();
            }

            var foodEntry = await _context.FoodEntries.FindAsync(id);
            if (foodEntry == null)
            {
                return NotFound();
            }
            return View(foodEntry);
        }

        // POST: FoodEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserId,Quantity,Date,Calories,Proteins,Carbs,Fats")] FoodEntry foodEntry)
        {
            if (id != foodEntry.Id)
            {
                return NotFound();
            }
            if(foodEntry.Date == null)
                foodEntry.Date = DateTime.Now;
            if (foodEntry.Date > DateTime.Now)
            {
                TempData["error"] = "Date cannot be in the future!";
                return View(foodEntry);
            }

            if (foodEntry != null)
            {
                foodEntry.UserId = (int)session.GetInt32("ID");
                try
                {
                    _context.Update(foodEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodEntryExists(foodEntry.Id))
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
            return View(foodEntry);
        }

        // GET: FoodEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FoodEntries == null)
            {
                return NotFound();
            }

            var foodEntry = await _context.FoodEntries
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodEntry == null)
            {
                return NotFound();
            }

            return View(foodEntry);
        }

        // POST: FoodEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FoodEntries == null)
            {
                return Problem("Entity set 'CalorieTrackerContext.FoodEntries'  is null.");
            }
            var foodEntry = await _context.FoodEntries.FindAsync(id);
            if (foodEntry != null)
            {
                _context.FoodEntries.Remove(foodEntry);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodEntryExists(int id)
        {
          return _context.FoodEntries.Any(e => e.Id == id);
        }
    }
}
