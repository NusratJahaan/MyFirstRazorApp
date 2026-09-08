using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstRazorApp.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; } = new Student();

        public List<SelectListItem> CourseOptions { get; set; } = new();

        public IActionResult OnGet()
        {
            // Populate dropdown with enum values
            CourseOptions = Enum.GetValues(typeof(CourseType))
                .Cast<CourseType>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdown on error
                CourseOptions = Enum.GetValues(typeof(CourseType))
                    .Cast<CourseType>()
                    .Select(c => new SelectListItem
                    {
                        Value = c.ToString(),
                        Text = c.ToString()
                    }).ToList();
                return Page();
            }

            _context.Students.Add(Student);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}