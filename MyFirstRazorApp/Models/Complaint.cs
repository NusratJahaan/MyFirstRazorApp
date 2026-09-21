using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class Complaint : BaseEntity
    {
        public string ComplaintNumber { get; set; }
        public string DefendantName { get; set; }
        public string DefendantPhone { get; set; }
        public string DefendantAddress { get; set; }
        public string Description { get; set; }
    }
}
