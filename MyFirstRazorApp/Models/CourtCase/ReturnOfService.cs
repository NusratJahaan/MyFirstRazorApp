using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.CourtCase
{
    public class ReturnOfService : BaseEntity
    {
        [Required]
        public int WarrantId { get; set; }
        public virtual Warrant? Warrant { get; set; }

        [Display(Name = "Served Date")]
        public DateTime? ServedDate { get; set; }

        [Display(Name = "Service Status")]
        public ServiceStatus ServiceStatus { get; set; } = ServiceStatus.Pending;

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }
    }
}