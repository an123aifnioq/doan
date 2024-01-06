using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using doan.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace doan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly DataContext _context;

        public OrdersController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 10; // Điều chỉnh kích thước trang nếu cần

            var dataContext = _context.Orders
                .Include(o => o.Account)
                .Include(o => o.OrderStatus)
                .Include(o => o.PaymentStatus); ;

            // Áp dụng phân trang
            var pageNumber = page ?? 1;
            var pagedOrders = await dataContext.ToPagedListAsync(pageNumber, pageSize);

            return View(pagedOrders);
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int id)
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
        /*
        // GET: Admin/Orders/Create
        public IActionResult Create()
        {
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID");
            ViewData["OrderStatusID"] = new SelectList(_context.OrderStatuses, "OrderStatusID", "OrderStatusID");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,Code,CustomerName,Phone,Address,TotalAmount,Quantity,OrderStatusID,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,AccountID")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", order.AccountID);
            ViewData["OrderStatusID"] = new SelectList(_context.OrderStatuses, "OrderStatusID", "OrderStatusID", order.OrderStatusID);
            return View(order);
        }
        */
        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", order.AccountID);
            ViewData["OrderStatusID"] = new SelectList(_context.OrderStatuses, "OrderStatusID", "OrderStatusID", order.OrderStatusID);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,Code,CustomerName,Phone,Address,TotalAmount,Quantity,OrderStatusID,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,AccountID")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
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
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountID", order.AccountID);
            ViewData["OrderStatusID"] = new SelectList(_context.OrderStatuses, "OrderStatusID", "OrderStatusID", order.OrderStatusID);
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Account)
                .Include(o => o.OrderStatus)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'DataContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.OrderID == id)).GetValueOrDefault();
        }
    }
}
