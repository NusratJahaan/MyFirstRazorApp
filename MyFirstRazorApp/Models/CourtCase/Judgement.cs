using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.CourtCase
{
    public class Judgement : BaseEntity
    {
        [Required]
        public int OffenceId { get; set; }
        public virtual Offence? Offence { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Judgement Disposition")]
        public string JudgementDisposition { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Judgement Date")]
        public DateTime JudgementDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        [Display(Name = "Judge Name")]
        public string? JudgeName { get; set; }

        [Display(Name = "Signed Date")]
        public DateTime? SignedDate { get; set; }

        [StringLength(500)]
        [Display(Name = "Signature Info")]
        public string? SignatureInfo { get; set; }

        [Display(Name = "Is Finished")]
        public bool IsFinished { get; set; } = false;
    }
}