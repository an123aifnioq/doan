using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using doan.Models;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace doan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogCommentsController : Controller
    {
        private readonly DataContext _context;

        public BlogCommentsController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/BlogComments
        public async Task<IActionResult> CommentsForBlog(int? page, int? id)
        {
            if (page == null) page = 1;
            int pageSize = 1; // điều chỉnh bản ghi hiện trên 1 trang
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.BlogID == id);

            if (blog == null)
            {
                return NotFound();
            }

            // Lấy danh sách comment cho bài viết
            var comments = await _context.BlogComments
                .Include(c => c.Account)
                .Where(c => c.BlogID == id)
                .ToListAsync();

            ViewBag.BlogTitle = blog.Title;
            // phân trang
            var pageNumber = page ?? 1;
            var pagedComments = comments.ToPagedList(pageNumber, pageSize);

            return View(pagedComments);
        }
        /*
        // GET: Admin/BlogComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BlogComments == null)
            {
                return NotFound();
            }

            var blogComment = await _context.BlogComments
                .Include(b => b.Account)
                .Include(b => b.Blog)
                .FirstOrDefaultAsync(m => m.CommentID == id);
            if (blogComment == null)
            {
                return NotFound();
            }

            return View(blogComment);
        }
        
        // GET: Admin/BlogComments/Create
        public IActionResult Create()
        {
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID");
            ViewData["BlogID"] = new SelectList(_context.Blogs, "BlogID", "BlogID");
            return View();
        }

        // POST: Admin/BlogComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentID,Name,Phone,Email,CreatedDate,Detail,BlogID,IsActive,AccountID")] BlogComment blogComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", blogComment.AccountID);
            ViewData["BlogID"] = new SelectList(_context.Blogs, "BlogID", "BlogID", blogComment.BlogID);
            return View(blogComment);
        }

        // GET: Admin/BlogComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BlogComments == null)
            {
                return NotFound();
            }

            var blogComment = await _context.BlogComments.FindAsync(id);
            if (blogComment == null)
            {
                return NotFound();
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", blogComment.AccountID);
            ViewData["BlogID"] = new SelectList(_context.Blogs, "BlogID", "BlogID", blogComment.BlogID);
            return View(blogComment);
        }

        // POST: Admin/BlogComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentID,Name,Phone,Email,CreatedDate,Detail,BlogID,IsActive,AccountID")] BlogComment blogComment)
        {
            if (id != blogComment.CommentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogCommentExists(blogComment.CommentID))
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
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", blogComment.AccountID);
            ViewData["BlogID"] = new SelectList(_context.Blogs, "BlogID", "BlogID", blogComment.BlogID);
            return View(blogComment);
        }
        */

        // GET: Admin/BlogComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BlogComments == null)
            {
                return NotFound();
            }

            var blogComment = await _context.BlogComments
                .Include(b => b.Account)
                .Include(b => b.Blog)
                .FirstOrDefaultAsync(m => m.CommentID == id);
            if (blogComment == null)
            {
                return NotFound();
            }

            return View(blogComment);
        }

        // POST: Admin/BlogComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BlogComments == null)
            {
                return Problem("Entity set 'DataContext.BlogComments'  is null.");
            }
            var blogComment = await _context.BlogComments.FindAsync(id);
            if (blogComment != null)
            {
                _context.BlogComments.Remove(blogComment);
            }
            var blogtId = blogComment.BlogID; // Lấy id của sản phẩm liên quan đến đánh giá này

            await _context.SaveChangesAsync();
            return RedirectToAction("CommentsForBlog", "BlogComments", new { id = blogtId });
        }

        private bool BlogCommentExists(int id)
        {
          return (_context.BlogComments?.Any(e => e.CommentID == id)).GetValueOrDefault();
        }
    }
}
