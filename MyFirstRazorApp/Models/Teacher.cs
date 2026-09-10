using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class Teacher : BaseEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Teacher name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; } = string.Empty;

        public string? Specialization { get; set; }

        // Foreign Key to Course
        public int? CourseId { get; set; }

        // Navigation Property
        public virtual Course? Course { get; set; }
    }
}
