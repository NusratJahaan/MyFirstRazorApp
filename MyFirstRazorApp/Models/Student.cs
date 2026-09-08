using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    // Course enum
    public enum CourseType //Add value
    {
        Bangla,
        English,
        Math,
        Science,
    }

    public class Student : BaseEntity
    {
        [Key]
        public int Id { get; set; } //move it to the base

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [Display(Name = "Student Name")]
        public string Name { get; set; } = string.Empty; //?? 

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty; //?? //Use Regex Validation

        [Range(5, 100, ErrorMessage = "Age must be between 5 and 100")]
        [Display(Name = "Age")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public CourseType Course { get; set; }
    }
}