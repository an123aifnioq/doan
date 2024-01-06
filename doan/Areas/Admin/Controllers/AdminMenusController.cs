using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using doan.Areas.Admin.Models;
using doan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace doan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMenusController : Controller
    {
        private readonly DataContext _context;

        public AdminMenusController(DataContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "1")]

        // GET: Admin/AdminMenus
        public IActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            var mnList = _context.AdminMenus.OrderByDescending(i => i.AdminMenuID).ToPagedList((int)page, pageSize);
            return View(mnList);

        }

        // GET: Admin/AdminMenus/Create
        public IActionResult Create()
        {
            var mnList = (from m in _context.AdminMenus
                          select new SelectListItem()
                          {
                              Text = m.ItemName,
                              Value = m.AdminMenuID.ToString(),
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = "0"
            });
            ViewBag.mnList = mnList;
            return View();
        }

        // POST: Admin/AdminMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminMenuID,ItemName,ItemLevel,ParentLevel,ItemOrder,IsActive,AreaName,ControllerName,ActionName,Icon")] AdminMenu adminMenu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminMenu);
        }

        // GET: Admin/AdminMenus/Edit/5
        public  IActionResult Edit(long? id)
        {
            if (id == null || _context.AdminMenus == null)
            {
                return NotFound();
            }

            var adminMenu = _context.AdminMenus.Find(id);
            if (adminMenu == null)
            {
                return NotFound();
            }

            var mnList = (from m in _context.AdminMenus
                          select new SelectListItem()
                          {
                              Text = m.ItemName,
                              Value = m.AdminMenuID.ToString(),
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            ViewBag.mnList = mnList;
            return View(adminMenu);
        }

        // POST: Admin/AdminMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AdminMenuID,ItemName,ItemLevel,ParentLevel,ItemOrder,IsActive,AreaName,ControllerName,ActionName,Icon")] AdminMenu adminMenu)
        {
            if (id != adminMenu.AdminMenuID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminMenu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminMenuExists(adminMenu.AdminMenuID))
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
            return View(adminMenu);
        }

        // GET: Admin/AdminMenus/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.AdminMenus == null)
            {
                return NotFound();
            }

            var adminMenu = await _context.AdminMenus
                .FirstOrDefaultAsync(m => m.AdminMenuID == id);
            if (adminMenu == null)
            {
                return NotFound();
            }

            return View(adminMenu);
        }

        // POST: Admin/AdminMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.AdminMenus == null)
            {
                return Problem("Entity set 'DataContext.AdminMenus'  is null.");
            }
            var adminMenu = await _context.AdminMenus.FindAsync(id);
            if (adminMenu != null)
            {
                _context.AdminMenus.Remove(adminMenu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminMenuExists(long id)
        {
          return (_context.AdminMenus?.Any(e => e.AdminMenuID == id)).GetValueOrDefault();
        }
    }
}
