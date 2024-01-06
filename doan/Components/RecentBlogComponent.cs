using doan.Models;
using Microsoft.AspNetCore.Mvc;

namespace doan.Components
{
    [ViewComponent(Name = "RecentBlog")]
    public class RecentPostComponent : ViewComponent
    {
        private readonly DataContext _context;
        public RecentPostComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofBlog = (from p in _context.Blogs
                              where (p.IsActive == true)
                              orderby p.BlogID descending
                              select p).Take(6).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofBlog));
        }
    }
}
