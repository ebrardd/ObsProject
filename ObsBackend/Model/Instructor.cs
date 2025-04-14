using System.ComponentModel.DataAnnotations.Schema;

namespace ObsBackend.Model
{
    [Table("Instructor")]
    public class Instructor : User
    {
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("surname")]
        public string Surname { get; set; } = string.Empty;
        
        [Column("Department")]
        public string Department { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}