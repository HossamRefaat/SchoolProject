using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using System.Reflection;

namespace SchoolProject.Infrastructure.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext()
        {

        }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Department> departments { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<DepartmentSubject> departmentSubjects { get; set; }
        public DbSet<StudentSubject> studentSubjects { get; set; }
        public DbSet<Subject> subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudID); //[key]

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
