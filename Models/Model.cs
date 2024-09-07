namespace PROG6212_MVC.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public string Contractor { get; set; }
        public string Project { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime SubmissionDate { get; set; }
    }

    public class Contractor
    {
        public int ContractorId { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Project { get; set; }
    }

    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }

}
