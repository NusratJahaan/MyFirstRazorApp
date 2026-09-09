using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly IStudentService _studentService;

        public DeleteModel(IStudentService studentService)
        {
            _studentService=studentService;
        }

        [BindProperty]
        public Student Student { get; set; } = new Student();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _studentService.GetStudentByIdAsync(id) ?? new Student();

            if (Student == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _studentService.DeleteStudentAsync(id);

            return RedirectToPage("./Index");
        }
    }
}
