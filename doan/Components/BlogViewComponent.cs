using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using doan.Models;

namespace doan.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly DataContext _context;

        public BlogViewComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = _context.Blogs.Where(m => (bool)m.IsActive).OrderByDescending(m=>m.BlogID).Take(5).ToList();
            ViewBag.blogComment = _context.BlogComments.ToList();
            return await Task.FromResult<IViewComponentResult>(View(items));
        }
    }
}
