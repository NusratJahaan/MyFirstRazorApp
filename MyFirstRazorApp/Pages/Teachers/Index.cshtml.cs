using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Teachers
{
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
            Teachers = await _teacherService.GetAllTeachersAsync();
        }
    }
}
