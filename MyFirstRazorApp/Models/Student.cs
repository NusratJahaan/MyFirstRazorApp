namespace MyFirstRazorApp.Models
{
    //BaseClass
    // CreatedDate
    //CreatedBy
    //UpdatedDate
    //UpdatedBy

    public class Student
    {
        public int Id { get; set; } //Primary Key
        public string Name { get; set; } //Required hote hbe
        public string Email { get; set; } //Valid Email hote hbe [Regex]
        public int? Age { get; set; } //nullable
        public string Course { get; set; } //Enum :: Bangla, English, Math, Science
    }
}