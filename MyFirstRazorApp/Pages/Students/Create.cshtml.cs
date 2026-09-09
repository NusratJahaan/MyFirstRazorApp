using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Services;
using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly IStudentService _studentService;

        public CreateModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [BindProperty]
        public Student Student { get; set; } = new Student();
        public IActionResult OnGet()
        {

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _studentService.AddStudentAsync(Student);

            return RedirectToPage("./Index");
        }
    }
}