using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBa.Models
{
    public class Message
    {
        [Key]
        public int MessageId {  get; set; }
        
        public int SenderId { get; set; }
        
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }

    }
}
