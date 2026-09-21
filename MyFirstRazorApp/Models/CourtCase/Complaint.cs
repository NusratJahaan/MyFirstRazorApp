using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.CourtCase
{
    public class Complaint : BaseEntity
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Case Number")]
        public string CaseNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Complainant Name")]
        public string ComplainantName { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "Complainant Phone")]
        public string? ComplainantPhone { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Defendant Name")]
        public string DefendantName { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Defendant Address")]
        public string? DefendantAddress { get; set; }

        [StringLength(20)]
        [Display(Name = "Defendant Phone")]
        public string? DefendantPhone { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Navigation
        public virtual ICollection<Offence> Offences { get; set; } = new List<Offence>();
        public virtual ICollection<Warrant> Warrants { get; set; } = new List<Warrant>();
    }
}
