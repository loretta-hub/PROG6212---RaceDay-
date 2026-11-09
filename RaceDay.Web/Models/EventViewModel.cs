using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class EventViewModel
    {
        public int EventID { get; set; }

        public int OrganiserID { get; set; }

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
        public string? BannerImageUrl { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Event Type")]
        public int EventTypeID { get; set; }

        public List<EventTypeOption> EventTypes { get; set; } = new();

        // Categories available for this event
        public List<CategoryOption> Categories { get; set; } = new();
    }

    public class EventTypeOption
    {
        public int EventTypeID { get; set; }

        public string TypeName { get; set; } = string.Empty;
    }

    public class CategoryOption
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Display(Name = "Event Banner")]
        public string? BannerImageUrl { get; set; }
    }
}
