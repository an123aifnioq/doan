using doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace doan.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(DataContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            ViewBag.categories = _context.Categories.ToList();
            ViewBag.products = _context.Products.ToList();
            ViewBag.productCategories = _context.ProductCategories.ToList();
            return View();
        }   
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
    }
}