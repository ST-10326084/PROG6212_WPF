using Microsoft.AspNetCore.Mvc;

namespace PROG6212_MVC.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
