using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using doan.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace doan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly DataContext _context;

        public BlogController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page, string search)
        {
            if (page == null) page = 1;
            int pageSize = 10;

            // Lấy tất cả bài viết từ cơ sở dữ liệu
            var allBlogs = _context.Blogs.Include(b => b.Category).OrderByDescending(p => p.BlogID);

            // Kiểm tra xem có yêu cầu tìm kiếm không
            if (!string.IsNullOrEmpty(search))
            {
                // Nếu có yêu cầu tìm kiếm, lọc danh sách bài viết
                allBlogs = (IOrderedQueryable<Blog>)allBlogs.Where(i => i.Title.ToLower().Contains(search.ToLower()));
            }

            // Áp dụng phân trang sau khi lọc hoặc không lọc
            var pagedBlogs = allBlogs.ToPagedList((int)page, pageSize);

            // Truyền danh sách bài viết (đã được phân trang) đến view
            return View(pagedBlogs);
        }

        public async Task<IActionResult> ListBlogCategory(int categoryID)
        {
            // Lấy danh sách sản phẩm theo danh mục
            var blog = await _context.Blogs
                .Where(a => a.CategoryID == categoryID)
                .Include(a => a.Category)
                .ToListAsync();

            return View("Index", blog);
        }
        public async Task<IActionResult> Details(int? id)
        {
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

            return View(blog);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var mn = _context.Blogs.Find(id);
            if (mn == null)
            {
                return NotFound();
            }
            return View(mn);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var deletedPost = _context.Blogs.Find(id);

            if (deletedPost == null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(deletedPost);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogID,Title,Alias,CategoryID,Description,Detail,Image,SeoTitle,SeoDescription,SeoKeywords,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", blog.CategoryID);
            return View(blog);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }

            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Title", blog.CategoryID);
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogID,Title,Alias,CategoryID,Description,Detail,Image,SeoTitle,SeoDescription,SeoKeywords,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive")] Blog blog)
        {
            if (id != blog.BlogID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogID))
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

            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", blog.CategoryID);
            return View(blog);
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs?.Any(e => e.BlogID == id) ?? false;
        }

    }
}
