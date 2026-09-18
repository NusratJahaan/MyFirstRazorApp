using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Teachers
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ITeacherService _teacherService;

        public IndexModel(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        public IList<Teacher> Teachers { get; set; } = new List<Teacher>();

        public async Task OnGetAsync()
        {
            try
            {
                Teachers = await _teacherService.GetAllTeachersAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Teachers = new List<Teacher>();
            }
        }
    }
}