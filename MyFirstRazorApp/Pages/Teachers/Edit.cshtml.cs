using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Teachers
{
    public class EditModel : PageModel
    {
        private readonly ITeacherService _teacherService;

        public EditModel(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = new Teacher();

        public List<SelectListItem> CourseOptions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Teacher = await _teacherService.GetTeacherByIdAsync(id);
                if (Teacher == null)
                {
                    return NotFound();
                }


                var courses = await _teacherService.GetAllCoursesAsync();
                foreach (var course in courses)
                {
                    CourseOptions.Add(new SelectListItem
                    {
                        Value = course.Id.ToString(),
                        Text = course.Name,
                        Selected = course.Id == Teacher.CourseId
                    });
                }

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting student: {ex.Message}");
                return Page();
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {

                var result = await _teacherService.UpdateTeacherAsync(Teacher);
                if (result)
                {
                    return RedirectToPage("./Index");
                }

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting student: {ex.Message}");
                return Page();
            }
        }
    }
}