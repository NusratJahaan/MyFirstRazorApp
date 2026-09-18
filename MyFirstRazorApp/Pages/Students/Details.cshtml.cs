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
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;

        public DetailsModel(IStudentService studentService, ITeacherService teacherService)
        {
            _studentService = studentService;
            _teacherService = teacherService;
        }

        public Student Student { get; set; } = new Student();
        public List<Course> EnrolledCourses { get; set; } = new();
        public string CurrentRole { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
                CurrentRole = role;

                // Role-based access
                if (role == "Student")
                {
                    // Student can only view their own details
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                    if (!int.TryParse(userIdClaim, out int userId))
                    {
                        return Forbid();
                    }

                    var ownStudent = await _studentService.GetStudentBySystemUserIdAsync(userId);
                    if (ownStudent == null || ownStudent.Id != id)
                    {
                        return Forbid();
                    }
                }
                else if (role == "Teacher")
                {
                    // Teacher can only view students in their courses
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                    if (!int.TryParse(userIdClaim, out int userId))
                    {
                        return Forbid();
                    }

                    var teacher = await _teacherService.GetTeacherBySystemUserIdAsync(userId);
                    if (teacher == null)
                    {
                        return Forbid();
                    }

                    // Check if student is in teacher's courses
                    var studentsInCourses = await _teacherService.GetStudentsInMyCoursesAsync(teacher.Id);
                    if (!studentsInCourses.Any(s => s.Id == id))
                    {
                        return Forbid();
                    }
                }
                // Coordinator → full access

                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    return NotFound();
                }

                Student = student;
                EnrolledCourses = await _studentService.GetEnrolledCoursesAsync(id);

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