using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models
{
    public class EventType
    {
        [Key]
        public int EventTypeID { get; set; }

        [Required]
        public string TypeName { get; set; }
    }
}