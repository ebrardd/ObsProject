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

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}