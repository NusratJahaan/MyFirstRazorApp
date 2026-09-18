using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class Teacher : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Teacher Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        // Link to login user
        public int? SystemUserId { get; set; }
        public SystemUser? SystemUser { get; set; }

        // One teacher → many courses
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}