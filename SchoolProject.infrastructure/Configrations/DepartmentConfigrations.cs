using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Data.Entities;

namespace SchoolProject.Infrastructure.Configrations
{
    internal class DepartmentConfigrations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {

            builder.HasKey(d => d.DID);

            builder.Property(ds => ds.DNameAr)
                .HasMaxLength(500);

            builder.HasMany(d => d.Students)
                .WithOne(s => s.Department)
                .HasForeignKey(s => s.DID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Instructor)
                .WithOne(i => i.DepartmentManger)
                .HasForeignKey<Department>(d => d.InstructorManager)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
