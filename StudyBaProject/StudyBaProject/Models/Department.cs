using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBa.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
