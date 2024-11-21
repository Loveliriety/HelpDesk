using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASI.Basecode.WebApp.Models
{
    public class TicketViewModel
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Status { get; set; }

        [Required]
        [MaxLength(255)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(100)]
        public string RequesterEmail { get; set; }

        [MaxLength(100)]
        public string Assignee { get; set; }

        [Required]
        public string Priority { get; set; }

        public string RequestedAt { get; set; }
        public string UpdatedAt { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
    }

    // You can also define Category if needed.
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int Count { get; set; }

        //working
    }
}
