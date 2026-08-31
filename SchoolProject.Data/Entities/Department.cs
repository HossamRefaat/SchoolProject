using SchoolProject.Data.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public partial class Department : LocalizalbeEntity
    {
        public Department()
        {
            Students = new HashSet<Student>();
            DepartmentSubjects = new HashSet<DepartmentSubject>();
            Instructors = new HashSet<Instructor>();
        }

        [Key]
        public int DID { get; set; }

        [StringLength(500)]
        public string? DNameAr { get; set; }

        [StringLength(500)]
        public string? DNameEn { get; set; }

        public int? InstructorManager { get; set; }

        [InverseProperty(nameof(Student.Department))]
        public virtual ICollection<Student> Students { get; set; }

        [InverseProperty(nameof(DepartmentSubject.Department))]
        public virtual ICollection<DepartmentSubject> DepartmentSubjects { get; set; }

        [InverseProperty(nameof(Instructor.Department))]
        public virtual ICollection<Instructor> Instructors { get; set; }

        [ForeignKey(nameof(InstructorManager))]
        [InverseProperty(nameof(Instructor.DepartmentManger))]
        public virtual Instructor? Instructor { get; set; }
    }
}
