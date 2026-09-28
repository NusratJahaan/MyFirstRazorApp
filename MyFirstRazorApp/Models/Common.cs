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
        public const string Index = "/";

        // Auth
        public const string Login = "/Account/Login";
        public const string Logout = "/Account/Logout";
        public const string Register = "/Account/Register";

        // Complaint
        public const string ComplaintList = "/CourtCase/Complaints/List";
        public const string ComplaintAdd = "/CourtCase/Complaints/Add";
        public const string ComplaintEdit = "/CourtCase/Complaints/Edit";
        public const string ComplaintDetails = "/CourtCase/Complaints/Details";
        public const string ComplaintManage = "/CourtCase/Complaints/Manage";

        // Offence
        public const string OffenceList = "/CourtCase/Offences/List";
        public const string OffenceAdd = "/CourtCase/Offences/Add";
        public const string OffenceEdit = "/CourtCase/Offences/Edit";
        public const string OffenceDetails = "/CourtCase/Offences/Details";

        // Witness
        public const string WitnessList = "/CourtCase/Witnesses/List";
        public const string WitnessAdd = "/CourtCase/Witnesses/Add";
        public const string WitnessEdit = "/CourtCase/Witnesses/Edit";

        // Warrant
        public const string WarrantList = "/CourtCase/Warrants/List";
        public const string WarrantAdd = "/CourtCase/Warrants/Add";
        public const string WarrantEdit = "/CourtCase/Warrants/Edit";
        public const string WarrantDetails = "/CourtCase/Warrants/Details";

        // Return of Service
        public const string ReturnOfServiceAdd = "/CourtCase/ReturnOfServices/Add";

        // Judgement
        public const string JudgementList = "/CourtCase/Judgements/List";
        public const string JudgementAdd = "/CourtCase/Judgements/Add";
        public const string JudgementEdit = "/CourtCase/Judgements/Edit";
        public const string JudgementDetails = "/CourtCase/Judgements/Details";
    }
}
