using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.CourtCase
{
    public class CaseHistory : BaseEntity
    {
        [Required]
        public int OffenceId { get; set; }
        public virtual Offence? Offence { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Action")]
        public string Action { get; set; } = string.Empty;

        [Display(Name = "Action Date")]
        public DateTime ActionDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }
    }
}