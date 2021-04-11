using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MercyProject.Data;
using MercyProject.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MercyProject.Controllers
{
    public class GalleriesController : Controller
    {
        private readonly MercyContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GalleriesController(MercyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this._webHostEnvironment = webHostEnvironment;
        }

        // GET: Galleries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Galleries.ToListAsync());
        }

        // GET: Galleries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleries = await _context.Galleries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (galleries == null)
            {
                return NotFound();
            }

            return View(galleries);
        }

        // GET: Galleries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Galleries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image")] Galleries galleries)
        {
            if (ModelState.IsValid)
            {

                string webroothpath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(galleries.Image.FileName);
                string extension = Path.GetExtension(galleries.Image.FileName);
                string path = Path.Combine(webroothpath + "/images/", fileName);
               galleries.Photo=fileName+fileName + extension;
                using (var filestream= new FileStream(path, FileMode.Create))
                {
                    await galleries.Image.CopyToAsync(filestream);
                }
                _context.Add(galleries);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(galleries);
        }

        // GET: Galleries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleries = await _context.Galleries.FindAsync(id);
            if (galleries == null)
            {
                return NotFound();
            }
            return View(galleries);
        }

        // POST: Galleries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Photo")] Galleries galleries)
        {
            if (id != galleries.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(galleries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleriesExists(galleries.Id))
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
            return View(galleries);
        }

        // GET: Galleries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleries = await _context.Galleries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (galleries == null)
            {
                return NotFound();
            }

            return View(galleries);
        }

        // POST: Galleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var galleries = await _context.Galleries.FindAsync(id);
            _context.Galleries.Remove(galleries);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalleriesExists(int id)
        {
            return _context.Galleries.Any(e => e.Id == id);
        }
    }
}
