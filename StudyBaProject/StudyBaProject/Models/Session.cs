using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBa.Models
{
    public class Session
    {
        [Key]
        public int SessionId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SessionDate { get; set; }
    }
}
