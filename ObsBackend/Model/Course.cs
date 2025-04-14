using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ObsBackend.Model;

[Table("Course")]
public class Course
{
    [Key]
    [Column("code")]
    public string Code { get; set; }

    [Column("lectureName")]
    public string LectureName { get; set; }

    [Column("instructorId")]
    public int InstructorId { get; set; }

    [ForeignKey("InstructorId")]
    public Instructor Instructor { get; set; } = null!;

    public ICollection<ResitExam> ResitExams { get; set; } = new List<ResitExam>();
}

