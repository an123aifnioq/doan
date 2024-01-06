using doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Security.Claims;
using X.PagedList;

namespace doan.Controllers
{
    public class BlogController : Controller
    {
        private readonly DataContext _context;
        public BlogController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 1;
            var blogs = _context.Blogs.Where(i => (bool)i.IsActive).OrderByDescending(i => i.BlogID).ToPagedList((int)page, pageSize);
            ViewBag.blogComment = _context.BlogComments.ToList();
            return View(blogs);
        }
        [Route("/blog/{alias}-{id}.html")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.BlogID == id);
            if (blog == null)
            {
                return NotFound();
            }

            ViewBag.blogComment = _context.BlogComments
                .Include(c => c.Account) // Đảm bảo nạp thông tin người đăng nhập cho bình luận
                .Where(i => i.BlogID == id)
                .ToList();

            return View(blog);
        }


        [HttpPost]
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
                        BlogComment comment = new BlogComment();
                        comment.BlogID = id;
                        comment.Detail = message;
                        comment.CreatedDate = DateTime.Now;

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
                    BlogComment comment = new BlogComment();
                    comment.BlogID = id;
                    comment.Detail = message;
                    comment.CreatedDate = DateTime.Now;
                    comment.Name = name;
                    comment.Phone = phone;
                    comment.Email = email;

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



    }
}