using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.CourtCase
{
    public class Warrant : BaseEntity
    {
        [Required]
        public int ComplaintId { get; set; }
        public virtual Complaint? Complaint { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Warrant Number")]
        public string WarrantNumber { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Warrant Type")]
        public string? WarrantType { get; set; }

        [Display(Name = "Case Numbers")]
        public string? CaseNumbers { get; set; }

        [Display(Name = "Issued Date")]
        public DateTime IssuedDate { get; set; } = DateTime.Now;

        [Display(Name = "Status")]
        public WarrantStatus Status { get; set; } = WarrantStatus.Issued;

        [Display(Name = "Is Finished")]
        public bool IsFinished { get; set; } = false;

        // Navigation
        public virtual ICollection<ReturnOfService> ReturnsOfService { get; set; } = new List<ReturnOfService>();
    }
}