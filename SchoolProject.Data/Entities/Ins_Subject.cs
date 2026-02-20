using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class Ins_Subject
    {
        [Key]
        public int InsId { get; set; }
        [Key]
        public int SubId { get; set; }

        [ForeignKey(nameof(InsId))]
        [InverseProperty(nameof(Instructor.Ins_Subjects))]
        public Instructor? Instructor { get; set; }

        [ForeignKey(nameof(SubId))]
        public Subject? Subject { get; set; }

    }
}
