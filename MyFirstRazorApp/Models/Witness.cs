using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class Witness : BaseEntity
    {
        public int ComplaintId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsVictim { get; set; }
        public string Statement { get; set; }
    }
}
