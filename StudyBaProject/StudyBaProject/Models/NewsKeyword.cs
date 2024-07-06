using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBa.Models
{
    public class NewsKeyword
    {
        [Key]
        public int KeywordId {  get; set; }
        public int KeywordValue { get; set; }
        [ForeignKey("News")]
        public int NewsId { get; set; }
        public News News { get; set; }
    }
}
