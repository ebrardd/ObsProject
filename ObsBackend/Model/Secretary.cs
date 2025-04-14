
using System.ComponentModel.DataAnnotations.Schema;

namespace ObsBackend.Model;

[Table("Secretary")]
public class Secretary : User
{
    [Column("name")]
    public string Name { get; set; }

    [Column("surname")]
    public string Surname { get; set; }
    
    [Column("id")]
    public int id { get; set; }
}