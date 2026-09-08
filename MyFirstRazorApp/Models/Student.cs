using System.ComponentModel.DataAnnotations;
namespace MyFirstRazorApp.Models
{
    // Course enum
    public enum CourseType
    {
        Bangla = 1,
        English = 2,
        Math = 3,
        Science = 4
    }

    public class Student : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [Display(Name = "Student Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Range(5, 100, ErrorMessage = "Age must be between 5 and 100")]
        [Display(Name = "Age")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public CourseType Course { get; set; }
    }
}