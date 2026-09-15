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
        public Student Student { get; set; } = new Student();

        public List<SelectListItem> CourseOptions { get; set; } = new();

        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public CreateModel(IStudentService studentService, ICourseService courseService)
        {
            _studentService = studentService;
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
                await _studentService.AddStudentAsync(Student);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await LoadCoursesAsync();
                return Page();
            }
        }

        private async Task LoadCoursesAsync() //bad
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
            ViewData["CourseOptions"] = CourseOptions;
        }
    }
}