using Microsoft.AspNetCore.Mvc;

namespace PROG6212_MVC.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // Hard-coded values for demo purposes
            var dashboardData = new
            {
                PendingClaims = new[]
                {
                    new { Id = 1, Description = "Claim 1", Amount = "$500" },
                    new { Id = 2, Description = "Claim 2", Amount = "$300" }
                },
                ApprovedClaims = new[]
                {
                    new { Id = 3, Description = "Claim 3", Amount = "$700" },
                    new { Id = 4, Description = "Claim 4", Amount = "$450" }
                },
                RejectedClaims = new[]
                {
                    new { Id = 5, Description = "Claim 5", Amount = "$250" }
                }
            };

            ViewBag.DashboardData = dashboardData;
            return View();
        }
    }
}
