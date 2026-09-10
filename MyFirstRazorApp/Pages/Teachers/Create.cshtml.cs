using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Teachers
{
    public class CreateModel : PageModel
    {
        private readonly ITeacherService _teacherService;

        public CreateModel(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = new Teacher();

        public List<SelectListItem> CourseOptions { get; set; } = new();

        public async Task OnGetAsync()
        {
            var courses = await _teacherService.GetAllCoursesAsync();
            foreach (var course in courses)
            {
                CourseOptions.Add(new SelectListItem
                {
                    Value = course.Id.ToString(),
                    Text = course.Name
                });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var result = await _teacherService.AddTeacherAsync(Teacher);
            if (result)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
