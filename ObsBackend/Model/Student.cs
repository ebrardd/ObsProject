using System.ComponentModel.DataAnnotations.Schema;

namespace ObsBackend.Model
    { 
        [Table("Student")]
        public class Student : User
        {
            [Column("name")]
            public string Name { get; set; }

            [Column("surname")]
            public string Surname { get; set; }

            [Column("department")]
            public string Department { get; set; }
        }
    }
