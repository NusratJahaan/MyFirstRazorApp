using System;
using System.Collections.Generic;

namespace MyFirstRazorApp.Models.Dtos
{
    public class PendingOffenceSearchDto
    {
        public string FileNumber { get; set; }
        public string DefendantName { get; set; }
        public int? OffenceLookUpId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<PendingOffenceDto> Results { get; set; } = new List<PendingOffenceDto>();
    }
}