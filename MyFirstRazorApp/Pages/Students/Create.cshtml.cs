using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Students
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Student Student { get; set; }

        public List<SelectListItem> CourseOptions { get; set; }

        private readonly IStudentService _studentService;

        public CreateModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task OnGetAsync()
        {
            Student = new();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                await _studentService.AddStudentAsync(Student);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Page();
            }
        }

    }
}