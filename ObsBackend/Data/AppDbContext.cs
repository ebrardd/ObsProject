using ObsBackend.Model;
using Microsoft.EntityFrameworkCore;


namespace ObsBackend.Data {

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Secretary> Secretaries { get; set; } 
} 
} 