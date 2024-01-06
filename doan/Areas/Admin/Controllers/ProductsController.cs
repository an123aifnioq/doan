using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using doan.Models;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace doan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }
        //GET: Admin/Product
        public IActionResult Index(int? page, string search)
        {
            if (page == null) page = 1;
            int pageSize = 10;

            // Lấy tất cả sản phẩm từ cơ sở dữ liệu
            var allProducts = _context.Products.Include(b => b.CategoryProduct).OrderByDescending(p => p.ProductID);

            // Kiểm tra xem có yêu cầu tìm kiếm không
            if (!string.IsNullOrEmpty(search))
            {
                // Nếu có yêu cầu tìm kiếm, lọc danh sách sản phẩm
                allProducts = (IOrderedQueryable<Product>)allProducts.Where(i => i.Title.ToLower().Contains(search.ToLower()));
            }

            // Áp dụng phân trang sau khi lọc hoặc không lọc
            var pagedProducts = allProducts.ToPagedList((int)page, pageSize);

            // Truyền danh sách sản phẩm (đã được phân trang) đến view
            return View(pagedProducts);
        }

        public async Task<IActionResult> ListProductCategory(int categoryProductID)
        {
            // Lấy danh sách sản phẩm theo danh mục
            var product = await _context.Products
                .Where(a => a.CategoryProductID == categoryProductID)
                .Include(a => a.CategoryProduct)
                .ToListAsync();

            return View("Index", product);
        }
        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var Product = await _context.Products
                .Include(t => t.CategoryProduct)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryProductID"] = new SelectList(_context.ProductCategories, "CategoryProductID", "Title");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Title,Alias,CategoryProductID,Description,Detail,Image,Price,PriceSale,Quantity,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsNew,IsBestSeller,UnitInStock,IsActive,Rating")] Product Product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryProductID"] = new SelectList(_context.ProductCategories, "CategoryProductID", "CategoryProductID", Product.CategoryProductID);
            return View(Product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var Product = await _context.Products.FindAsync(id);
            if (Product == null)
            {
                return NotFound();
            }
            ViewData["CategoryProductID"] = new SelectList(_context.ProductCategories, "CategoryProductID", "Title", Product.CategoryProductID);
            return View(Product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Title,Alias,CategoryProductID,Description,Detail,Image,Price,PriceSale,Quantity,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsNew,IsBestSeller,UnitInStock,IsActive,Rating")] Product Product)
        {
            if (id != Product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(Product.ProductID))
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
            ViewData["CategoryProductID"] = new SelectList(_context.ProductCategories, "CategoryProductID", "CategoryProductID", Product.CategoryProductID);
            return View(Product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var Product = await _context.Products
                .Include(t => t.CategoryProduct)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'DataContext.Products'  is null.");
            }
            var Product = await _context.Products.FindAsync(id);
            if (Product != null)
            {
                _context.Products.Remove(Product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductID == id)).GetValueOrDefault();
        }
    }
}
