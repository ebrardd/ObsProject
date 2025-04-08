using System.ComponentModel.DataAnnotations.Schema;


namespace ObsBackend.Model
{
    [Table("Instructor")]
    public class Instructor :User
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("surname")]
        public string Surname { get; set; }
        
    }
}
