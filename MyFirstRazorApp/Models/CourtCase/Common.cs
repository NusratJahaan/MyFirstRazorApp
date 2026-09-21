namespace MyFirstRazorApp.Models.CourtCase
{
    public enum OffenceStatus
    {
        Pending = 1,
        Approved = 2,
        Declined = 3,
    }

    public enum  CaseStatus
    {
        Inactive = 0,
        Active = 1,
    }

    public enum ServiceStatus
    {
        Pending = 1,
        Served = 2,
        NotServed = 3
    }

    public class OffenceLookUp //Seed Data
    {
        public int Id { get; set; }
        public string Code { get; set; } // 20-141
        public string Description { get; set; } // Spped Offence
    }
}