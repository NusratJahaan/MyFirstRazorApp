using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        private readonly ITeacherService _teacherService;

        public IndexModel(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        public IList<Teacher> Teachers { get; set; } = new List<Teacher>();

        public async Task OnGetAsync()
        {
            try
            {
                Teachers = await _teacherService.GetAllTeachersAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting student: {ex.Message}");
            }
        }
    }
}
