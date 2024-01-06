using doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace doan.Controllers
{
    public class OrderController : Controller
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy AccountID từ thông tin người dùng đăng nhập
            var accountId = GetAccountId();

            // Lấy danh sách đơn hàng của người dùng hiện tại
            var orders = _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.PaymentStatus)
                .Where(o => o.AccountID == accountId)  // Chỉ lấy đơn hàng của AccountID hiện tại
                .OrderByDescending(o => o.CreatedDate)
                .Take(10)
                .ToList();

            return View(orders);
        }
        public async Task<IActionResult> CancelOrder(int id)
        {
            var accountId = GetAccountId();

            // Lấy đơn hàng cần hủy
            var order = await _context.Orders
                .Include(o => o.OrderStatus)
                .FirstOrDefaultAsync(o => o.OrderID == id && o.AccountID == accountId);

            if (order == null)
            {
                return NotFound();
            }

            // Chỉ hủy đơn nếu đơn hàng đang ở trạng thái cho phép hủy (trạng thái "Chờ xác nhận" chẳng hạn)
            if (order.OrderStatusID == 6)
            {
                // Thực hiện các bước hủy đơn hàng, ví dụ:
                // 1. Đặt trạng thái hủy đơn
                order.OrderStatusID = 5;
                // 2. Cập nhật ngày và người hủy đơn
                order.ModifiedDate = DateTime.Now;
                order.ModifiedBy = User.Identity.Name;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Xử lý khi đơn hàng không thể hủy (có thể thông báo lỗi)
                TempData["ErrorMessage"] = "Không thể hủy đơn hàng này.";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order.OrderDetails);
        }


        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderDetailID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderDetails == null)
            {
                return Problem("Entity set 'DataContext.OrderDetails'  is null.");
            }
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private int? GetAccountId()
        {
            // Lấy thông tin người dùng đăng nhập từ User.Identity
            var userName = User.Identity.Name;

            // Lấy AccountID từ CSDL hoặc bất kỳ cơ chế nào mà hệ thống xác thực của bạn cung cấp
            var account = _context.Accounts.FirstOrDefault(a => a.UserName == userName);

            // Kiểm tra xem có tìm thấy tài khoản không
            return account?.AccountID;
        }


    }
}
