using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC.Models;

namespace PROG6212_MVC.Controllers
{
    public class ClaimsController : Controller
    {
        public ActionResult Index()
        {
            // Fetch the list of claims.
            var claims = GetClaims();
            return View(claims);
        }

        private List<Claim> GetClaims()
        {
            // Sample claims data
            return new List<Claim>
        {
            new Claim { ClaimId = 1, Contractor = "ABC Ltd.", Project = "Project A", Amount = 5000, Status = "Pending", SubmissionDate = DateTime.Now },
            new Claim { ClaimId = 2, Contractor = "XYZ Inc.", Project = "Project B", Amount = 7000, Status = "Approved", SubmissionDate = DateTime.Now }
        };
        }
    }

}
