using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context; // DB Context call hobe Data folder er AppDbContext class theke


        public IList<Student> Students { get; set; } = new List<Student>();


        public IndexModel(AppDbContext context)
        {
            _context = context;
        }


        public async Task OnGetAsync()
        {
            Students = await _context.Students.ToListAsync();
        }
    }
}

// Learn 
// Dependcy Injection er maddhome AppDbContext class er instance ke IndexModel class e inject kora hoyeche.
// Ekhane _context variable er maddhome database er Students table theke data fetch kora hocche.
// OnGetAsync method ta asynchronous vabe Students list ke populate kore.