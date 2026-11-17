
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReturnFlow.Models
{
    public class ReturnRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderId { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string RequestType { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Reason { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }

        // Product inspection information

        [StringLength(30)]
        public string? ProductCondition { get; set; }

        [StringLength(1000)]
        public string? InspectionFindings { get; set; }

        [StringLength(30)]
        public string? Disposition { get; set; }

        public DateTime? InspectedAt { get; set; }
    }
}
