using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC.Models;

namespace PROG6212_MVC.Controllers
{
    public class ContractorsController : Controller
    {
        public ActionResult Contractors()
        {
            // Fetch the list of contractors.
            var contractors = GetContractors();
            return View(contractors);
        }

        private List<Contractor> GetContractors()
        {
            // Sample contractors data
            return new List<Contractor>
        {
            new Contractor { ContractorId = 1, Name = "ABC Ltd.", Contact = "abc@example.com", Project = "Project A" },
            new Contractor { ContractorId = 2, Name = "XYZ Inc.", Contact = "xyz@example.com", Project = "Project B" }
        };
        }
    }

}
