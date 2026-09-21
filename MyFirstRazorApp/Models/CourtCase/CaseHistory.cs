using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.CourtCase
{
    public class CaseHistory : BaseEntity
    {
        public int OffenceId { get; set; }
        public virtual Offence Offence { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Action")]
        public string Action { get; set; }

        public CaseStatus CaseStatus { get; set; }

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
    }
}