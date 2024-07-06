using StudyBa.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StudyBaProject.Models
{
    public class SessionRequest
    {
        [Key]
        public int SessionRequestId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public required String SenderContact {  get; set; }
        public String Message { get; set; }
        public string Subject { get; set; }

    }
}
