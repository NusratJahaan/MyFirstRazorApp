using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IList<Course> Courses { get; set; } = new List<Course>();

        public async Task OnGetAsync()
        {
            try
            {
                Courses = await _courseService.GetAllCoursesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Courses = new List<Course>();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _courseService.DeleteCourseAsync(id);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while deleting.");
                await OnGetAsync();
                return Page();
            }
        }
    }
}