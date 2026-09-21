using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models.StudentModels
{
    public class SystemUser : BaseEntity
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        [Display(Name = "Role")]
        public string Role { get; set; } = string.Empty;
    }
}