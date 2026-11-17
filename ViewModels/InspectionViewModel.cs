using System.ComponentModel.DataAnnotations;

namespace ReturnFlow.Models.ViewModels
{
    public class InspectionViewModel
    {
        [Required]
        [StringLength(30)]
        public string RequestId { get; set; } = string.Empty;

        [Required]
        public string Condition { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Findings { get; set; } = string.Empty;

        [Required]
        public string Disposition { get; set; } = string.Empty;
    }
}
