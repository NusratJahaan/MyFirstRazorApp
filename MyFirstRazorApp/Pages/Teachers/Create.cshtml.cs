using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Teachers
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Teacher Teacher { get; set; } = new Teacher();

        public List<SelectListItem> CourseOptions { get; set; } = new();

        private readonly ITeacherService _teacherService;
        private readonly ICourseService _courseService;

        public CreateModel(ITeacherService teacherService, ICourseService courseService)
        {
            _teacherService = teacherService;
            _courseService = courseService;
        }
        public async Task OnGetAsync()
        {
            await LoadCoursesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCoursesAsync();
                return Page();
            }

            try
            {
                await _teacherService.AddTeacherAsync(Teacher);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating teacher: {ex.Message}");
                ModelState.AddModelError("", "An error occurred. Please try again.");
                await LoadCoursesAsync();
                return Page();
            }
        }

        private async Task LoadCoursesAsync()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();
                foreach (var course in courses)
                {
                    CourseOptions.Add(new SelectListItem
                    {
                        Value = course.Id.ToString(),
                        Text = course.Name
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading courses: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while loading courses.");
            }
            ViewData["CourseOptions"] = CourseOptions;
        }
    }
}