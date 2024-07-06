using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBa.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        
        public int TutorId { get; set; }
     
        public int StudentId { get; set;}
        public string Comment { get; set;}
        public int Grade { get; set;}
        public User Tutor { get; set; }
        public User Student { get; set; }

    }
}
