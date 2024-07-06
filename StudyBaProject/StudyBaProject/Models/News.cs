using System.ComponentModel.DataAnnotations;

namespace StudyBa.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string SourceLink { get; set; }
    }
}
