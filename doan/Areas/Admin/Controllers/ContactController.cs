using doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace doan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly DataContext _context;
        public ContactController(DataContext context)
        {

            _context = context;
        }
        public IActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            ViewBag.Message = TempData["Message"];

            var contact = _context.Contacts.ToList().ToPagedList((int)page, pageSize);
            return View(contact);
        }
        public IActionResult Feedback(int? id)
        {
            var contact = _context.Contacts.Find(id);

            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }
        [HttpPost]
        public IActionResult Feedback(int id)
        {
            var contact = _context.Contacts.Find(id);

            if (ModelState.IsValid)
            {
            _context.Contacts.Add(contact);
            _context.SaveChanges();
            
            TempData["Message"] = "Phản hồi đã được gửi thành công.";
            return RedirectToAction("Index");
            }
            return View(contact);
        }
        public IActionResult Delete(int? id)
        {
            var contact = _context.Contacts.Find(id);

            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var contact = _context.Contacts.Find(id);

            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
