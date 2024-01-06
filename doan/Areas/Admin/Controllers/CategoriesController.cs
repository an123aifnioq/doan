using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using doan.Models;
using doan.Utilities;
using X.PagedList;

namespace doan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        public IActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 5;
            var cate = _context.Categories.OrderBy(i => i.CategoryID).ToPagedList((int)page, pageSize);
            return View(cate);
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var Category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (Category == null)
            {
                return NotFound();
            }

            return View(Category);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,Title,Alias,Description,Position,SeoTitle,SeoDescription,SeoKeywords,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Category Category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Category);
        }

        // GET: Admin/Categories/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var Category = await _context.Categories.FindAsync(id);
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,Title,Alias,Description,Position,SeoTitle,SeoDescription,SeoKeywords,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Category Category)
        {
            if (id != Category.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(Category.CategoryID))
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
            return View(Category);
        }

        // GET: Admin/Categories/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var mn = _context.Categories.Find(id);
            if (mn == null)
            {
                return NotFound();
            }
            return View(mn);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var deleCategory = _context.Categories.Find(id);
            if (deleCategory == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(deleCategory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.CategoryID == id)).GetValueOrDefault();
        }
    }
}
