using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Data.Entities;

namespace SchoolProject.Infrastructure.Configrations
{
    internal class StudentSubjectConfigraions : IEntityTypeConfiguration<StudentSubject>
    {
        public void Configure(EntityTypeBuilder<StudentSubject> builder)
        {
            builder
            .HasKey(ss => new
            {
                ss.SubID,
                ss.StudID
            });

            builder.HasOne(d => d.Student)
                      .WithMany(ds => ds.StudentSubjects)
                      .HasForeignKey(d => d.StudID);

            builder.HasOne(d => d.Subject)
                      .WithMany(ds => ds.StudentsSubjects)
                      .HasForeignKey(d => d.SubID);
        }
    }
}
