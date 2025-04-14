
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObsBackend.Model
{
    [Table("ResitExam")]
    public class ResitExam
    {
        [Key]
        [Column("id")]
        public int Id { get; set; } 

        [Column("resitDate")]
        public string? ResitDate { get; set; }

        [Column("resitTime")]
        public string? ResitTime { get; set; }

        [Column("Location")]
        public string? Location { get; set; }

        [Column("courseCode")]
        public string CourseCode { get; set; }

        [Column("Department")]
        public string Department { get; set; }

        [Column("LecturerId")]
        public int LecturerId { get; set; }

        [ForeignKey("CourseCode")]
        public virtual Course Course { get; set; }

        [ForeignKey("LecturerId")]
        public virtual Instructor Instructor { get; set; }
    }
}


