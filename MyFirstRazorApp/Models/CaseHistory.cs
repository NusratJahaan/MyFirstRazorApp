using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class CaseHistory : BaseEntity
    {
        public int OffenceId { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public CaseStatus CaseStatus { get; set; }
        public string Remarks { get; set; }
    }
}
