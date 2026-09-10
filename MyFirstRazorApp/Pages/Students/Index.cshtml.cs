using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly IStudentService _studentService;


        public IList<Student> Students { get; set; } = new List<Student>();


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