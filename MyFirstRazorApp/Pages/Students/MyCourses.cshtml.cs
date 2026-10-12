using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MyFirstRazorApp.Models.StudentModels;
using MyFirstRazorApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Students
{
    [Authorize(Roles = Roles.Student)]
    public class MyCoursesModel : PageModel
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<MyCoursesModel> _logger;

        public MyCoursesModel(IStudentService studentService, ILogger<MyCoursesModel> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        public List<Course> EnrolledCourses { get; set; } = new();
        public List<Course> AvailableCourses { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var student = await GetCurrentStudentAsync();
                if (student == null) return Forbid();

                EnrolledCourses = await _studentService.GetEnrolledCoursesAsync(student.Id);
                AvailableCourses = await _studentService.GetAvailableCoursesAsync(student.Id);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading courses");
                return RedirectToPage("/Students/Index");
            }
        }

        // Enroll in a course
        public async Task<IActionResult> OnPostEnrollAsync(int courseId)
        {
            try
            {
                var student = await GetCurrentStudentAsync();
                if (student == null) return Forbid();

                await _studentService.EnrollAsync(student.Id, courseId);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enrolling in course {CourseId}", courseId);
                return RedirectToPage();
            }
        }

        // Unenroll from a course
        public async Task<IActionResult> OnPostUnenrollAsync(int courseId)
        {
            try
            {
                var student = await GetCurrentStudentAsync();
                if (student == null) return Forbid();

                await _studentService.UnenrollAsync(student.Id, courseId);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unenrolling from course {CourseId}", courseId);
                return RedirectToPage();
            }
        }

        private async Task<Student?> GetCurrentStudentAsync()
        {
            var userId = ClaimsHelper.GetUserId(User);
            if (userId == null) return null;

            return await _studentService.GetStudentBySystemUserIdAsync(userId.Value);
        }
    }
}