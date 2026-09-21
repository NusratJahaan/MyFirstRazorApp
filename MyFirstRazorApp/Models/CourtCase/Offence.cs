using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.CourtCase
{
    public class Offence : BaseEntity
    {
        [Required]
        public int ComplaintId { get; set; }
        public virtual Complaint? Complaint { get; set; }

        [Required]
        [Display(Name = "Offence Type")]
        public int OffenceTypeId { get; set; }
        public virtual OffenceType? OffenceType { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Status")]
        public OffenceStatus Status { get; set; } = OffenceStatus.Draft;

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; } = false;

        [Display(Name = "Is Finished")]
        public bool IsFinished { get; set; } = false;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Navigation
        public virtual ICollection<Witness> Witnesses { get; set; } = new List<Witness>();
        public virtual ICollection<Judgement> Judgements { get; set; } = new List<Judgement>();
        public virtual ICollection<CaseHistory> CaseHistories { get; set; } = new List<CaseHistory>();
    }
}