using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBa.Models
{
    public class AdministratorSubject
    {
        [Key]
        public int AdministratorSubjectId { get; set; }
        [ForeignKey("User")]
        public int AdministratorId { get; set; }
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public User User { get; set; }
        public Subject Subject { get; set; }

    }
}
