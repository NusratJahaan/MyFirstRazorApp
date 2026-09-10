using System.ComponentModel.DataAnnotations;
namespace MyFirstRazorApp.Models
{
    public class Teacher : BaseEntity
    {
        [Required(ErrorMessage = "Teacher name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;
        public string? Specialization { get; set; }

        // Foreign Key to Course
        public int? CourseId { get; set; }

        // Navigation Property
        public virtual Course? Course { get; set; }
    }
}
