using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Students
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Student Student { get; set; } = new Student();

        public List<SelectListItem> CourseOptions { get; set; } = new();
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public EditModel(IStudentService studentService, ICourseService courseService)
        {
            _studentService = studentService;
            _courseService = courseService;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _studentService.GetStudentByIdAsync(id) ?? new Student();

            if (Student.Id == 0)
            {
                return NotFound();
            }

            await LoadCoursesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCoursesAsync();
                return Page();
            }

            var existingStudent = await _studentService.GetStudentByIdAsync(Student.Id);
            existingStudent.Name = Student.Name;
            existingStudent.Email = Student.Email;
            existingStudent.Age = Student.Age;
            existingStudent.CourseId = Student.CourseId;
            existingStudent.UpdatedDate = DateTime.Now;

            try
            {
                await _studentService.UpdateStudentAsync(existingStudent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await LoadCoursesAsync();
                return Page();
            }

            return RedirectToPage("./Index");
        }

        private async Task LoadCoursesAsync()
        {
            var courses = await _courseService.GetAllCoursesAsync();

            ViewData["CourseOptions"] = courses.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }
    }
}