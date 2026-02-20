using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Data.Entities;

namespace SchoolProject.Infrastructure.Configrations
{
    public class Ins_SubjectConfigrations : IEntityTypeConfiguration<Ins_Subject>
    {
        public void Configure(EntityTypeBuilder<Ins_Subject> builder)
        {
            builder
                .HasKey(ds => new { ds.InsId, ds.SubId });

            builder.HasOne(d => d.Instructor)
                  .WithMany(ds => ds.Ins_Subjects)
                  .HasForeignKey(d => d.InsId);

            builder.HasOne(d => d.Subject)
                  .WithMany(ds => ds.Ins_Subjects)
                  .HasForeignKey(d => d.SubId);
        }
    }
}
