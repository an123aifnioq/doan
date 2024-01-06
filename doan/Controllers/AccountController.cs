using doan.Models;
using doan.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Security.Principal;

namespace doan.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Accounts.Include(a => a.Role);
            return View(await dataContext.ToListAsync());
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Accounts.SingleOrDefault(u => u.UserName == username);

            if (user == null || !VerifyPassword(password, user.Password))
            {
                // Tên người dùng hoặc mật khẩu không hợp lệ
                Functions._Message = "Sai tên đăng nhập hoặc mật khẩu!";
                return View("Login");
            }
            Functions._Message = string.Empty;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.RoleID.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            return RedirectToAction("Index", "Home");
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            // Sử dụng hàm mã hóa
            string enteredPasswordHash = HashMD5.GetHash(enteredPassword);

            // So sánh mật khẩu đã nhập với mật khẩu đã lưu
            return enteredPasswordHash == storedPasswordHash;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string username, string password, string fullName, string phone, string email)
        {
            try
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                if (_context.Accounts.Any(u => u.UserName == username))
                {
                    Functions._Message = "Tài khoản đã tồn tại!";
                    return View(); // Trả về trang đăng ký với thông báo lỗi
                }

                // Tạo mới tài khoản
                var newUser = new Account
                {
                    UserName = username,
                    Password = password,
                    FullName = fullName,
                    Phone = phone,
                    Email = email,
                    IsActive = true,
                    RoleID = 2, // Gán giá trị mặc định cho RoleID                           
                };                
                Functions._Message = string.Empty;

                newUser.Password = HashMD5.GetHash(newUser.Password != null ? newUser.Password : "");
                // Thêm tài khoản mới vào cơ sở dữ liệu
                _context.Accounts.Add(newUser);
                _context.SaveChanges();

                // Tạo claims để xác thực người dùng
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, newUser.UserName),
                    new Claim(ClaimTypes.Role, newUser.RoleID.ToString()),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Đăng nhập người dùng mới đăng ký
                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity)
                );

                return RedirectToAction("Index", "Home"); // Chuyển hướng sau khi đăng ký thành công
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (hiển thị hoặc ghi vào log)
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi đăng ký.");
                return View();
            }
        }

        // Thêm action cho trang đổi mật khẩu
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword)
        {
            var username = User.Identity.Name;
            var user = _context.Accounts.SingleOrDefault(u => u.UserName == username);

            if (user == null || user.Password != oldPassword)
            {
                ModelState.AddModelError(string.Empty, "Invalid old password.");
                return View();
            }

            // Cập nhật mật khẩu mới
            user.Password = newPassword;
            _context.SaveChanges();

            // Đăng nhập lại với mật khẩu mới nếu muốn
            // ...

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Profile()
        {
            var username = User.Identity.Name;
            var user = _context.Accounts.SingleOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return RedirectToAction("Index", "Home"); // Hoặc có thể chuyển hướng đến một trang thông báo lỗi
            }

            return View(user);
        }
        public IActionResult UpdateProfile()
        {
            return View();
        }
        // Cập nhật thông tin người dùng
        [HttpPost]
        public IActionResult UpdateProfile(string fullName, string phone, string email)
        {
            var username = User.Identity.Name;
            var user = _context.Accounts.SingleOrDefault(u => u.UserName == username);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View();
            }

            // Cập nhật thông tin người dùng
            user.FullName = fullName;
            user.Phone = phone;
            user.Email = email;
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
