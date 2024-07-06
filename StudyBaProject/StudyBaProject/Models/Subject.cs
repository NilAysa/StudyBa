using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBa.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public string SubjectName { get; set; }
        
        public Department Department { get; set; }

    }
}
