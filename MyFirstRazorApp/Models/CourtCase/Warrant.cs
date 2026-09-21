using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.CourtCase
{
    public class Warrant : BaseEntity
    {
        [Required]
        public int ComplaintId { get; set; }
        public virtual Complaint? Complaint { get; set; }

        [Display(Name = "Case Numbers")]
        public string CaseNumbers { get; set; }

        [Display(Name = "Issued Date")]
        public DateTime IssuedDate { get; set; }

        //Official Info
        public string OfficialName { get; set; }
        public DateTime OfficialSignDate { get; set; }

        [Display(Name = "Is Finished")]
        public bool FinishedAndLocked { get; set; }

    }
}