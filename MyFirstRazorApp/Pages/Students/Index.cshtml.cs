using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly IStudentService _studentService;


        public IList<Student> Students { get; set; } = new List<Student>();


        public IndexModel(IStudentService studentService)
        {
            _studentService = studentService;
        }


        public async Task OnGetAsync()
        {
            Students = await _studentService.GetAllStudentsAsync();
        }
    }
}