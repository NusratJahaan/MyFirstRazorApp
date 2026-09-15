using System.ComponentModel.DataAnnotations;
namespace MyFirstRazorApp.Models
{
    public class Course : BaseEntity
    {
        [Required(ErrorMessage = "Course name is required")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; }

        // One Course has many Students
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();

        // One Course has one Teacher
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Teacher? Teacher { get; set; }

    }
}