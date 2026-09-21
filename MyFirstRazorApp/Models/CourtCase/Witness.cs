using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.CourtCase
{
    public class Witness : BaseEntity
    {
        [Required]
        public int OffenceId { get; set; }
        public virtual Offence? Offence { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [StringLength(500)]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Is Victim")]
        public bool IsVictim { get; set; } = false;

        [Display(Name = "Statement")]
        public string? Statement { get; set; }
    }
}