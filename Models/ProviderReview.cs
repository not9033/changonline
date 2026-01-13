using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangOnline.Models
{
    public class ProviderReview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [ForeignKey("ProviderId")]
        public ProviderProfile? Provider { get; set; }

        [Required]
        public required string ReviewerId { get; set; }

        [ForeignKey("ReviewerId")]
        public AppUser? Reviewer { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
