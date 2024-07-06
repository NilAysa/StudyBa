using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBa.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public String IdentityId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string? ContactNumber { get; set; }
        public string Role { get; set; }
        public string Avatar { get; set; }
    }
}
