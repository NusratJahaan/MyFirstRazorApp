using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class Coordinator : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [Display(Name = "Coordinator Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [Display(Name = "Department")]
        public string? Department { get; set; }
    }
}
