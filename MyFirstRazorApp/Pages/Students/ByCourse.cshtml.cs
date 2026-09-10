using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Students
{
    public class ByCourseModel : PageModel
    {
        private readonly IStudentService _studentService;

        public ByCourseModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public IList<Student> Students { get; set; } = new List<Student>();
        public string CourseName { get; set; } = string.Empty;

        public async Task OnGetAsync(string course)
        {
            try
            {
                CourseName = course;
                Students = await _studentService.GetStudentsByCourseAsync(course);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting student: {ex.Message}");
            }
        }
    }
}