using doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doan.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly DataContext _context;

        public CheckOutController(DataContext context)
        {
            _context = context;
        }
        public IActionResult CheckOut()
        {
            var cartItems = GetCartItems();
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(string customerName, string phone, string address)
        {
            var cartItems = GetCartItems();
            var accountId = GetAccountId();

            // Kiểm tra giỏ hàng và tạo đơn hàng nếu có sản phẩm
            var order = await CreateOrder(accountId, customerName, phone, address, cartItems);

            if (order != null)
            {
                order.OrderStatusID = 6; // Gán trạng thái "Chờ duyệt" cho đơn hàng
                order.PaymentStatusID = 1;
                order.ModifiedDate = DateTime.Now;
                order.ModifiedBy = User.Identity.Name;

                // Lưu đơn hàng và chi tiết đơn hàng vào cơ sở dữ liệu
                _context.Update(order);
                await _context.SaveChangesAsync();

                // Xóa các sản phẩm đã được đặt hàng từ giỏ hàng (hoặc thực hiện các bước khác tùy thuộc vào yêu cầu)
                ClearCart();

                return RedirectToAction("Index", "Order");
            }

            // Xử lý khi không có sản phẩm trong giỏ hàng (có thể thông báo lỗi)
            return RedirectToAction("CheckoutError");
        }

        private async Task<Order> CreateOrder(int? accountId, string customerName, string phone, string address, List<CartItem> cartItems)
        {
            if (cartItems.Any())
            {
                // Tính tổng giá trị đơn hàng
                int totalAmount = (int)cartItems.Sum(item => item.quantity * item.product.Price);

                // Tạo đơn hàng
                var order = new Order
                {
                    AccountID = accountId,
                    CustomerName = customerName,
                    Phone = phone,
                    Address = address,
                    CreatedDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    OrderDetails = cartItems.Select(item => new OrderDetail
                    {
                        ProductID = item.product.ProductID,
                        Quantity = item.quantity,
                        Price = item.product.Price
                    }).ToList()
                };

                // Lưu đơn hàng vào cơ sở dữ liệu
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return order;
            }

            return null; // Trả về null nếu giỏ hàng trống
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

        public const string CARTKEY = "cart";

        // Lấy cart từ Session (danh sách CartItem)
        List<CartItem> GetCartItems()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }
    }
}
