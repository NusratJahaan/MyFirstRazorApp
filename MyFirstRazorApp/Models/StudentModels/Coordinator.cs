using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.StudentModels
{
    public class Coordinator : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Coordinator Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        // Link to login user
        public int? SystemUserId { get; set; }
        public SystemUser? SystemUser { get; set; }
    }
}