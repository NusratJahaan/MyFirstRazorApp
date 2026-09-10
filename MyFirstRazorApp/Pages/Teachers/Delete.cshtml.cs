using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Teachers
{
    public class DeleteModel : PageModel
    {
        private readonly ITeacherService _teacherService;

        public DeleteModel(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = new Teacher();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Teacher = await _teacherService.GetTeacherByIdAsync(id);
                if (Teacher == null)
                {
                    return NotFound();
                }
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting student: {ex.Message}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var result = await _teacherService.DeleteTeacherAsync(id);
            if (result)
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
