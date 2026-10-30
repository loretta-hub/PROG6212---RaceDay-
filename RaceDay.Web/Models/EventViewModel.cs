
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class EventViewModel
    {
        public int EventID { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100000)]
        [Display(Name = "Distance (km)")]
        public decimal Distance { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Event Type")]
        public int EventTypeID { get; set; }
    }
}
