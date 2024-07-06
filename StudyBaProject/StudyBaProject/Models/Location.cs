using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBa.Models
{
    public class Location
    {
        [Key]
        public int LocationId {  get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public User User { get; set; }
    }
}
