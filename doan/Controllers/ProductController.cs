using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using doan.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;
using System.Drawing.Printing;

namespace doan.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        public ProductController(DataContext context)
        {
            _context = context;
        }
        
        [Route("/product/{alias}-{id}.html")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(i => i.CategoryProduct)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            // Lấy danh mục sản phẩm tương ứng với sản phẩm hiện tại
            var productCategory = _context.ProductCategories.FirstOrDefault(pc => pc.CategoryProductID == product.CategoryProductID);
            if (productCategory != null)
            {
                ViewBag.productCategories = new List<ProductCategory> { productCategory };
            }
            else
            {
                ViewBag.productCategories = new List<ProductCategory>();
            }

            ViewBag.productReview = _context.ProductReviews
                .Include(c => c.Account)
                .Where(i => i.ProductID == id)
                .ToList();
            ViewBag.productRelated = _context.Products.Where(i => i.ProductID != id && i.CategoryProductID == product.CategoryProductID).Take(5).OrderByDescending(i => i.ProductID).ToList();

            return View(product);
        }
        public async Task<IActionResult> Create(int? id, string name, string phone, string email, string message)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    // Lấy thông tin người dùng hiện tại
                    var currentUser = await _context.Accounts.FirstOrDefaultAsync(a => a.UserName == User.Identity.Name);

                    if (currentUser != null)
                    {
                        ProductReview comment = new ProductReview();
                        comment.ProductID = id;
                        comment.Detail = message;
                        comment.CreatedDate = DateTime.Now;
                        comment.IsActive = true;
                        comment.Star = 5;

                        // Kiểm tra và sử dụng thông tin tài khoản nếu có
                        comment.AccountID = currentUser.AccountID;
                        comment.Name = currentUser.FullName;
                        comment.Phone = currentUser.Phone;
                        comment.Email = currentUser.Email;

                        _context.Add(comment);
                        _context.SaveChanges();

                        return Json(new { status = true, comment = new { Account = new { Image = currentUser.Image }, comment.Name, comment.Detail } });
                    }
                    else
                    {
                        return Json(new { status = false, message = "Không thể xác định thông tin người dùng hiện tại." });
                    }
                }
                else
                {
                    // Người dùng chưa đăng nhập, tạo bình luận với thông tin cung cấp
                    ProductReview comment = new ProductReview();
                    comment.ProductID = id;
                    comment.Detail = message;
                    comment.CreatedDate = DateTime.Now;
                    comment.Name = name;
                    comment.Phone = phone;
                    comment.Email = email;
                    comment.IsActive = true;
                    comment.Star = 5;

                    _context.Add(comment);
                    _context.SaveChanges();

                    return Json(new { status = true, comment = new { Name = comment.Name, Detail = comment.Detail } });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = $"Đã xảy ra lỗi khi thêm bình luận: {ex.Message}" });
            }
        }

        public IActionResult DanhMuc1(int? page)
        {
            return DanhMuc(1, page);
        }

        public IActionResult DanhMuc2(int? page)
        {
            return DanhMuc(2, page);
        }

        public IActionResult DanhMuc3(int? page)
        {
            return DanhMuc(13, page);
        }

        public IActionResult DanhMuc4(int? page)
        {
            return DanhMuc(14, page);
        }
        public IActionResult DanhMuc(int categoryId, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 12;

            // Lọc sản phẩm theo danh mục có ID là categoryId
            var mnList = _context.Products
                .Include(i => i.CategoryProduct)
                .Where(i => i.CategoryProduct.CategoryProductID == categoryId && i.IsActive)
                .OrderByDescending(i => i.ProductID)
                .ToPagedList((int)page, pageSize);

            // Lấy tên của danh mục
            var categoryName = _context.ProductCategories.FirstOrDefault(c => c.CategoryProductID == categoryId)?.Title;

            // Gửi dữ liệu CategoryId và CategoryName về view
            ViewBag.CategoryId = categoryId;
            ViewBag.CategoryName = categoryName;

            return View(mnList);
        }


    }
}
