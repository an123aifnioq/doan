using doan.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace doan.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class HomeController : Controller
    {
        [Authorize(Roles = "1,3")]
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Logout()
        {
            Functions._AccountID = 0;
            Functions._UserName = string.Empty;
            Functions._Message = string.Empty;
            return RedirectToAction("Index", "Login");
        }
    }
}
