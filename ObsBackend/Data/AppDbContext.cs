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
    public DbSet<Course> Courses { get; set; } 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Instructor>().ToTable("Instructor");

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey("instructorId"); // STRING olarak! Çünkü küçük harfli kolon var

        base.OnModelCreating(modelBuilder);
    }



} 
} 