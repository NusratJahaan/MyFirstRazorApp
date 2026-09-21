using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models.StudentModels;
using MyFirstRazorApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Students
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IStudentService _studentService;

        public IndexModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public IList<Student> Students { get; set; } = new List<Student>();
        public string CurrentRole { get; set; } = string.Empty;
        public int CurrentStudentId { get; set; } = 0;

        public async Task OnGetAsync()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
                CurrentRole = role;

                if (role == "Student")
                {
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                    if (int.TryParse(userIdClaim, out int userId))
                    {
                        var student = await _studentService.GetStudentBySystemUserIdAsync(userId);
                        if (student != null)
                        {
                            Students.Add(student);
                            CurrentStudentId = student.Id;
                        }
                    }
                }
                else
                {
                    Students = await _studentService.GetAllStudentsAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Students = new List<Student>();
            }
        }
    }
}