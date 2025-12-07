using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangOnline.Models
{
    public class ServiceRequest
    {
        [Key] public int Id { get; set; }

        public required string Address { get; set; }

        public string? Subdistrict { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }

        public required DateTime AppointmentDate { get; set; }

        [Phone] public required string ContactNumber { get; set; }

        public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.Pending;

        public string? JobDetails { get; set; }

        public string? Remarks { get; set; }

        public required string RequesterId { get; set; }

        [ForeignKey("RequesterId")] public AppUser? Requester { get; set; }

        public required int ProviderId { get; set; }

        [ForeignKey("ProviderId")] public ProviderProfile? Provider { get; set; }
    }
}
