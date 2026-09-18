using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Teachers
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ITeacherService _teacherService;
        private readonly ICourseService _courseService;

        public DetailsModel(ITeacherService teacherService, ICourseService courseService)
        {
            _teacherService = teacherService;
            _courseService = courseService;
        }

        public Teacher Teacher { get; set; } = new Teacher();
        public List<Course> Courses { get; set; } = new();
        public Dictionary<int, List<Student>> CourseStudents { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherByIdAsync(id);

                if (teacher == null)
                {
                    return NotFound();
                }

                Teacher = teacher;
                Courses = await _teacherService.GetMyCoursesAsync(id);

                // Load students for each course
                foreach (var course in Courses)
                {
                    CourseStudents[course.Id] = await _courseService.GetStudentsInCourseAsync(course.Id);
                }

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