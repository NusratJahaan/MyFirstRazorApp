using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class Course : BaseEntity
    {
        [Required(ErrorMessage = "Course name is required")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Course Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        // One teacher per course (nullable — coordinator creates without teacher)
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        // Many-to-many with Student
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}