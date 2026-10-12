using Microsoft.AspNetCore.Authorization;
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
    public class IndexModel : PageModel
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IStudentService studentService, ILogger<IndexModel> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        public IList<Student> Students { get; set; } = new List<Student>();
        public string CurrentRole { get; set; } = string.Empty;
        public int CurrentStudentId { get; set; } = 0;

        public async Task OnGetAsync()
        {
            try
            {
                var role = ClaimsHelper.GetRole(User);
                CurrentRole = role;

                if (role == Roles.Student)
                {
                    var userId = ClaimsHelper.GetUserId(User);
                    if (userId != null)
                    {
                        var student = await _studentService.GetStudentBySystemUserIdAsync(userId.Value);
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
                _logger.LogError(ex, "Error loading students");
                Students = new List<Student>();
            }
        }
    }
}