using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using doan.Models;
using Microsoft.CodeAnalysis;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace doan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductReviewsController : Controller
    {
        private readonly DataContext _context;

        public ProductReviewsController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductReviews
        public async Task<IActionResult> CommentsForProduct(int? page, int? id)
        {
            if (page == null) page = 1;
            int pageSize = 10; // điều chỉnh bản ghi hiện trên 1 trang
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(t => t.CategoryProduct)
                .FirstOrDefaultAsync(m => m.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            // Lấy danh sách đánh giá cho sản phẩm
            var comments = await _context.ProductReviews
                .Include(c => c.Account)
                .Where(c => c.ProductID == id)
                .ToListAsync();

            ViewBag.ProductTitle = product.Title;

            // Áp dụng phân trang
            var pageNumber = page ?? 1;
            var pagedComments = comments.ToPagedList(pageNumber, pageSize);

            return View(pagedComments);
        }
        // GET: Admin/ProductReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductReviews == null)
            {
                return NotFound();
            }

            var productReview = await _context.ProductReviews
                .Include(p => p.Account)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductReviewID == id);
            if (productReview == null)
            {
                return NotFound();
            }

            return View(productReview);
        }

        // POST: Admin/ProductReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductReviews == null)
            {
                return Problem("Entity set 'DataContext.ProductReviews' is null.");
            }

            var productReview = await _context.ProductReviews.FindAsync(id);
            if (productReview == null)
            {
                return NotFound();
            }

            var productId = productReview.ProductID; // Lấy id của sản phẩm liên quan đến đánh giá này

            _context.ProductReviews.Remove(productReview);
            await _context.SaveChangesAsync();

            // Chuyển hướng về trang danh sách đánh giá của sản phẩm
            return RedirectToAction("CommentsForProduct", "ProductReviews", new { id = productId });
        }

        private bool ProductReviewExists(int id)
        {
          return (_context.ProductReviews?.Any(e => e.ProductReviewID == id)).GetValueOrDefault();
        }
    }
}
