using SchoolProject.Data.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class Instructor : LocalizalbeEntity
    {
        public Instructor()
        {
            Instructors = new HashSet<Instructor>();
            Ins_Subjects = new HashSet<Ins_Subject>();
        }

        [Key]
        public int InsId { get; set; }
        public string? ENameAr { get; set; }
        public string? ENameEn { get; set; }
        public string? Address { get; set; }
        public string? Position { get; set; }
        public int? SupervisorId { get; set; }
        public decimal? Salary { get; set; }
        public string? Image { get; set; }
        public int DID { get; set; }

        [ForeignKey(nameof(DID))]
        [InverseProperty(nameof(Department.Instructors))]
        public Department Department { get; set; }

        [InverseProperty(nameof(Department.Instructor))]
        public Department? DepartmentManger { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        [InverseProperty(nameof(Instructors))]
        public Instructor? Supervisor { get; set; }

        [InverseProperty(nameof(Supervisor))]
        public virtual ICollection<Instructor> Instructors { get; set; }

        [InverseProperty(nameof(Ins_Subject.Instructor))]
        public ICollection<Ins_Subject> Ins_Subjects { get; set; }
    }
}
