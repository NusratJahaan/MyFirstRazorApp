using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class ReturnOfService
    {
        public int WarrantId { get; set; }
        public DateTime ServedDate { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
        public string Remarks { get; set; }
    }
}
