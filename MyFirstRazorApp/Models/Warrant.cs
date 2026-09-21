using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class Warrant : BaseEntity
    {
        public int ComplaintId { get; set; }
        public string CaseNumbers { get; set; }
        public DateTime IssuedDate { get; set; }
        public string OfficialName { get; set; }
        public DateTime OfficialSignDate { get; set; }
        public bool FinishedAndLocked { get; set; }
    }
}
