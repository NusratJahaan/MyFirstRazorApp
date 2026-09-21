using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models.StudentModels;
using MyFirstRazorApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Courses
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICourseService _courseService;
        private readonly ITeacherService _teacherService;

        public IndexModel(ICourseService courseService, ITeacherService teacherService)
        {
            _courseService = courseService;
            _teacherService = teacherService;
        }

        public IList<Course> AllCourses { get; set; } = new List<Course>();
        public IList<Course> MyCourses { get; set; } = new List<Course>();
        public IList<Course> AvailableCourses { get; set; } = new List<Course>();
        public string CurrentRole { get; set; } = string.Empty;
        public int CurrentTeacherId { get; set; } = 0;

        public async Task OnGetAsync()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
                CurrentRole = role;

                if (role == "Teacher")
                {
                    // Teacher → split courses
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                    if (int.TryParse(userIdClaim, out int userId))
                    {
                        var teacher = await _teacherService.GetTeacherBySystemUserIdAsync(userId);
                        if (teacher != null)
                        {
                            CurrentTeacherId = teacher.Id;
                            MyCourses = await _teacherService.GetMyCoursesAsync(teacher.Id);
                        }
                    }
                    AvailableCourses = await _teacherService.GetAvailableCoursesAsync();
                }
                else
                {
                    // Coordinator → all courses
                    AllCourses = await _courseService.GetAllCoursesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // ✅ Teacher Claims a Course
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> OnPostClaimAsync(int courseId)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (!int.TryParse(userIdClaim, out int userId)) return RedirectToPage();

                var teacher = await _teacherService.GetTeacherBySystemUserIdAsync(userId);
                if (teacher == null) return RedirectToPage();

                await _teacherService.ClaimCourseAsync(teacher.Id, courseId);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToPage();
            }
        }

        // ✅ Teacher Releases a Course
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> OnPostReleaseAsync(int courseId)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (!int.TryParse(userIdClaim, out int userId)) return RedirectToPage();

                var teacher = await _teacherService.GetTeacherBySystemUserIdAsync(userId);
                if (teacher == null) return RedirectToPage();

                await _teacherService.ReleaseCourseAsync(teacher.Id, courseId);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToPage();
            }
        }
    }
}