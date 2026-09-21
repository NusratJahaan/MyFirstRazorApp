namespace MyFirstRazorApp.Models.CourtCase
{
    public enum OffenceStatus
    {
        Draft = 1,
        Approved = 2,
        Declined = 3,
        Rollback = 4
    }

    public enum WarrantStatus
    {
        Issued = 1,
        Served = 2,
        Executed = 3,
        Cancelled = 4
    }

    public enum ServiceStatus
    {
        Pending = 1,
        Served = 2,
        NotServed = 3
    }
}