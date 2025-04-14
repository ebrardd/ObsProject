using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObsBackend.Model;
[Table("ExamAnnouncement")]
public class ExamAnnouncement
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("examId")]
    public int ExamId { get; set; }

    [Column("message")]
    public string Message { get; set; }

   
}