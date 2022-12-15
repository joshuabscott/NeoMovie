using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NeoMovie.Data;
using NeoMovie.Models.Database;
using NeoMovie.Models.Settings;

namespace NeoMovie.Controllers
{
    public class CollectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;

        public CollectionsController(ApplicationDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        // GET: Collections
        public async Task<IActionResult> Index()
        {
            var defaultCollectionName = _appSettings.NeoMovieSettings.DefaultCollection.Name;
            var collections = await _context.Collection.Where(c => c.Name != defaultCollectionName).ToListAsync();

            return View(collections);
        }

        /*Delete the Create GET, will be using a model instead*/


        // POST: Collections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Collection collection)
        {
            _context.Add(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "MovieCollections", new { id = collection.Id });
        }

        // GET: Collections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collection.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }

        // POST: Collections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Collection collection)
        {
            if (id != collection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (collection.Name == _appSettings.NeoMovieSettings.DefaultCollection.Name)
                    {
                        return RedirectToAction("Index", "Collections");
                    }

                    _context.Update(collection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollectionExists(collection.Id))
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
            return View(collection);
        }

        // GET: Collections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collection
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collection == null)
            {
                return NotFound();
            }

            if (collection.Name == _appSettings.NeoMovieSettings.DefaultCollection.Name)
            {
                return RedirectToAction("Index", "Collections");
            }

            return View(collection);
        }

        // POST: Collections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collection = await _context.Collection.FindAsync(id);
            _context.Collection.Remove(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "MovieCollections");
        }

        private bool CollectionExists(int id)
        {
            return _context.Collection.Any(e => e.Id == id);
        }
    }
}