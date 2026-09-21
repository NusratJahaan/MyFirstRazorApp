using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.CourtCase
{
    public class Offence : BaseEntity
    {
        public int ComplaintId { get; set; }
        public virtual Complaint Complaint { get; set; }
        public int OffenceTypeId { get; set; }

        public string FileNumber { get; set; } //Format || CR-Year-MM- UNIQ 


        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Status")]
        public OffenceStatus OffenceStatus { get; set; }

        public CaseStatus CaseStatus { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; }

    }
}