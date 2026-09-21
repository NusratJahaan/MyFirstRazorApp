using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models.StudentModels;
using MyFirstRazorApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Courses
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ICourseService _courseService;

        public DetailsModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public Course Course { get; set; } = new Course();
        public IList<Student> Students { get; set; } = new List<Student>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var course = await _courseService.GetCourseWithDetailsAsync(id);

                if (course == null)
                {
                    return NotFound();
                }

                Course = course;
                Students = await _courseService.GetStudentsInCourseAsync(id);

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound();
            }
        }
    }
}