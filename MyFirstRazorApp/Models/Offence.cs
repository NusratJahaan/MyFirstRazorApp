using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class Offence
    {
        public int ComplaintId { get; set; }
        public int OffenceLookUpId { get; set; }
        public string FileNumber { get; set; }
        public string Description { get; set; }
        public OffenceStatus OffenceStatus { get; set; }
        public CaseStatus CaseStatus { get; set; }
        public bool IsApproved { get; set; }
        public bool FinishedAndLocked { get; set; }

    }
}
