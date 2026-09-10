using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class Course : BaseEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // One Course has many Students
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();

        // One Course has one Teacher
        public virtual Teacher? Teacher { get; set; }
    }
}