namespace MyFirstRazorApp.Models
{
    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; } 
        public string? CreatedBy { get; set; } //? remove
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; } //? remove
    }
}