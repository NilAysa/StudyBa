using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBa.Models
{
    public class UserNews
    {
        [Key]
        public int UserNewsId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("News")]
        public int NewsId { get; set; }
        public User User { get; set; }
        public News News { get; set; }
    }
}
