using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.CourtCase
{
    public class Witness : BaseEntity
    {
        public int ComplaintId { get; set; }
        public virtual Complaint Complaint { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(20)]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [StringLength(500)]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Is Victim")]
        public bool IsVictim { get; set; }

        [Display(Name = "Statement")]
        public string? Statement { get; set; }
    }
}