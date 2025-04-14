using Microsoft.EntityFrameworkCore;
using ObsBackend.Model;

namespace ObsBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Secretary> Secretaries { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ResitExam> ResitExams { get; set; }
        
        public DbSet<ExamAnnouncement> ExamAnnouncements { get; set; }
        public DbSet<LetterGrade> LetterGrades { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");

            // Course - Instructor (FK: InstructorId → Instructor.Id)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId);

            // ResitExam - Course (FK: CourseCode → Course.Code)
            modelBuilder.Entity<ResitExam>()
                .HasOne(r => r.Course)
                .WithMany(c => c.ResitExams)
                .HasForeignKey(r => r.CourseCode)
                .HasPrincipalKey(c => c.Code); // <--- CRITICAL

            // ResitExam - Instructor (FK: LecturerId → Instructor.Id)
            modelBuilder.Entity<ResitExam>()
                .HasOne(r => r.Instructor)
                .WithMany()
                .HasForeignKey(r => r.LecturerId)
                .HasPrincipalKey(i => i.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}