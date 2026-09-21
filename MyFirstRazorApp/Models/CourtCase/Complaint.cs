using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.CourtCase
{
    public class Complaint : BaseEntity
    {

        [StringLength(50)]
        [Display(Name = "Complaint Number")]
        public string ComplaintNumber { get; set; } //Format || CR-Year-MM- UNIQ 

        [Required]
        [StringLength(100)]
        [Display(Name = "Defendant Name")]
        public string DefendantName { get; set; }

        [StringLength(500)]
        [Display(Name = "Defendant Address")]
        public string? DefendantAddress { get; set; }

        [StringLength(20)]
        [Display(Name = "Defendant Phone")]
        public string DefendantPhone { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }


    }
}
