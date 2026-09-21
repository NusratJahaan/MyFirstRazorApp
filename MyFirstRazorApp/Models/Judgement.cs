using System.ComponentModel.DataAnnotations;

namespace MyFirstRazorApp.Models
{
    public class Judgement : BaseEntity
    {
        public int OffenceId { get; set; }
        public string JudgementDisposition { get; set; }
        public string Description { get; set; }
        public DateTime JudgementDate { get; set; }
        public string JudgeName { get; set; }
        public DateTime SignedDate { get; set; }
        public string SignatureInfo { get; set; }
        public bool FinishedAndLocked { get; set; }

    }
}
