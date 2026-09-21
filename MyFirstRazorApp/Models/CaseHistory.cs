using MyFirstRazorApp.Models.CourtCase;
using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class CaseHistory
    {
        public int OffenceId { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public CaseStatus CaseStatus { get; set; }
        public string Remarks { get; set; }
    }
}
