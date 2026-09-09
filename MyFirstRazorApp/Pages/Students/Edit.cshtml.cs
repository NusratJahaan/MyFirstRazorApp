using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly IStudentService _studentService;

        public EditModel(IStudentService studentService)
        {
            _studentService = studentService;
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingStudent = await _studentService.GetStudentByIdAsync(Student.Id);
            existingStudent.Name = Student.Name;
            existingStudent.Email = Student.Email;
            existingStudent.Age = Student.Age;
            existingStudent.Course = Student.Course;
            existingStudent.UpdatedDate = DateTime.Now;
            await _studentService.UpdateStudentAsync(existingStudent);

            try
            {
                await _studentService.UpdateStudentAsync(existingStudent);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (Student.Id == 0)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}