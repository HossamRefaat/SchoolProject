using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Data.Entities;

namespace SchoolProject.Infrastructure.Configrations
{
    public class DepartmentSubjectConfigrations : IEntityTypeConfiguration<DepartmentSubject>
    {
        public void Configure(EntityTypeBuilder<DepartmentSubject> builder)
        {
            builder
                .HasKey(ds => new { ds.SubID, ds.DID });

            builder.HasOne(d => d.Department)
                      .WithMany(ds => ds.DepartmentSubjects)
                      .HasForeignKey(d => d.DID);

            builder.HasOne(d => d.Subject)
                  .WithMany(ds => ds.DepartmetsSubjects)
                  .HasForeignKey(d => d.SubID);
        }
    }
}
