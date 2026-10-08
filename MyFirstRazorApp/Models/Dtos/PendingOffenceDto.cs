using System;

namespace MyFirstRazorApp.Models.Dtos
{
    public class PendingOffenceDto
    {
        public int Id { get; set; }
        public string FileNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OffenceType { get; set; }
        public string DefendantName { get; set; }
        public string ComplaintNumber { get; set; }
    }
}