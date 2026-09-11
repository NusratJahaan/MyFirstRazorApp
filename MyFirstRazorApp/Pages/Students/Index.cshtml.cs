using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Students
{
    public class IndexModel : PageModel
    {
        public IList<Student> Students { get; set; } = new List<Student>();

        private readonly IStudentService _studentService;

        public IndexModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task OnGetAsync()
        {
            try
            {
                Students = await _studentService.GetAllStudentsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}