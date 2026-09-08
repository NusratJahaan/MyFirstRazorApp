using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstRazorApp.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; } = new Student();

        public List<SelectListItem> CourseOptions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _context.Students.FindAsync(id);

            if (Student == null)
            {
                return NotFound();
            }

            // Populate dropdown
            CourseOptions = Enum.GetValues(typeof(CourseType))
                .Cast<CourseType>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString(),
                    Selected = c == Student.Course
                }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                CourseOptions = Enum.GetValues(typeof(CourseType))
                    .Cast<CourseType>()
                    .Select(c => new SelectListItem
                    {
                        Value = c.ToString(),
                        Text = c.ToString()
                    }).ToList();
                return Page();
            }

            var existingStudent = await _context.Students.FindAsync(Student.Id);
            existingStudent.Name = Student.Name;
            existingStudent.Email = Student.Email;
            existingStudent.Age = Student.Age;
            existingStudent.Course = Student.Course;
            existingStudent.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StudentExistsAsync(Student.Id))
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

        private async Task<bool> StudentExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(e => e.Id == id);
        }
    }
}