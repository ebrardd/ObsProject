using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ObsBackend.Model;
[Table("Course")]
public class Course
{
    [Key]
    [Column("code")]
    public string Code { get; set; }

    [Column("lectureName")]
    public string Name { get; set; }

    [Column("instructorId")]
    [ForeignKey("Instructor")]
    
    public int instructorId { get; set; }

    public Instructor Instructor { get; set; }
}

