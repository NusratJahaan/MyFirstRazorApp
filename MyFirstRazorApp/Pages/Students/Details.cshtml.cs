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
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IStudentService studentService, ITeacherService teacherService, ILogger<DetailsModel> logger)
        {
            _studentService = studentService;
            _teacherService = teacherService;
            _logger = logger;
        }

        public Student Student { get; set; } = new Student();
        public List<Course> EnrolledCourses { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var role = ClaimsHelper.GetRole(User);

                // Role-based access
                if (role == Roles.Student)
                {
                    // Student can only view their own details
                    var userId = ClaimsHelper.GetUserId(User);
                    if (userId == null)
                    {
                        return Forbid();
                    }

                    var ownStudent = await _studentService.GetStudentBySystemUserIdAsync(userId.Value);
                    if (ownStudent == null || ownStudent.Id != id)
                    {
                        return Forbid();
                    }
                }
                else if (role == Roles.Teacher)
                {
                    // Teacher can only view students in their courses
                    var userId = ClaimsHelper.GetUserId(User);
                    if (userId == null)
                    {
                        return Forbid();
                    }

                    var teacher = await _teacherService.GetTeacherBySystemUserIdAsync(userId.Value);
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
                _logger.LogError(ex, "Error loading student details for student {StudentId}", id);
                return NotFound();
            }
        }
    }
}