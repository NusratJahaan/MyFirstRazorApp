using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Students
{
    [Authorize(Roles = "Student")]
    public class MyCoursesModel : PageModel
    {
        private readonly IStudentService _studentService;

        public MyCoursesModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public Student Student { get; set; } = new Student();
        public List<Course> EnrolledCourses { get; set; } = new();
        public List<Course> AvailableCourses { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var student = await GetCurrentStudentAsync();
                if (student == null) return Forbid();

                Student = student;
                EnrolledCourses = await _studentService.GetEnrolledCoursesAsync(student.Id);
                AvailableCourses = await _studentService.GetAvailableCoursesAsync(student.Id);

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToPage("/Students/Index");
            }
        }

        // ✅ Enroll in a course
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
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToPage();
            }
        }

        // ✅ Unenroll from a course
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
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToPage();
            }
        }

        // ---------- Helpers ----------

        private async Task<Student?> GetCurrentStudentAsync()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId)) return null;

            return await _studentService.GetStudentBySystemUserIdAsync(userId);
        }
    }
}