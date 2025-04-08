
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace ObsBackend.Model
    {
        [Table("User")] 
        public class User
        {
            [Required]
            [Column("email")] 
            public string Email { get; set; }

            [Required]
            [Column("password")]
            public string Password { get; set; }

            [Required]
            [Column("role")]
            public string Role { get; set; }
            [Key]
            [Column("id")]
            public int Id { get; set; }
        }
    }
