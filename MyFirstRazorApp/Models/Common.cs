namespace MyFirstRazorApp.Models
{
    public enum OffenceStatus
    {
        Pending = 1,
        Approved = 2,
        Declined = 3,
    }

    public enum CaseStatus
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

    //navigator
    // Navigator — central place for all navigation URLs
    public static class Navigator
    {
        public const string Index = "./Index";

        // Complaint
        public const string ComplaintList = "/CourtCase/Complaints/List";
        public const string ComplaintAdd = "/CourtCase/Complaints/Add";
        public const string ComplaintDetails = "/CourtCase/Complaints/Details";
        public const string ComplaintEdit = "/CourtCase/Complaints/Edit";

        // Future (for later)
        // public const string OffenceList = "/CourtCase/Offences/List";
        // public const string WarrantList = "/CourtCase/Warrants/List";
        // public const string WitnessList = "/CourtCase/Witnesses/List";
        // public const string JudgementList = "/CourtCase/Judgements/List";
    }
}
