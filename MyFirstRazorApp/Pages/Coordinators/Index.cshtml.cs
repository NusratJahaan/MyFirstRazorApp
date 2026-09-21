using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models.StudentModels;
using MyFirstRazorApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Coordinators
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICoordinatorService _coordinatorService;

        public IndexModel(ICoordinatorService coordinatorService)
        {
            _coordinatorService = coordinatorService;
        }

        public IList<Coordinator> Coordinators { get; set; } = new List<Coordinator>();

        public async Task OnGetAsync()
        {
            try
            {
                Coordinators = await _coordinatorService.GetAllCoordinatorsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Coordinators = new List<Coordinator>();
            }
        }
    }
}