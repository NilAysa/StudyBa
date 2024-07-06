using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBa.Models
{
    public class TutorSubject
    {
        [Key]
        public int TutorSubjectId { get; set; }
        [ForeignKey("User")]
        public int TutorId { get; set; }
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        public User User { get; set; }
        public Subject Subject { get; set; }


    }
}
