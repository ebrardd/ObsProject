using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObsBackend.Model
{
   
    [Table("LetterGrade")]
    public class LetterGrade
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("studentId")] 
        public int StudentId { get; set; }

        [Column("course")]
        public string Course { get; set; }

        [Column("grade")]
        public string Grade { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

       
        
    }


}