
using System.ComponentModel.DataAnnotations;

namespace ReturnFlow.Models.ViewModels
{
    public class ReturnRequestViewModel
    {
        [Required(ErrorMessage = "Order ID is required.")]
        [StringLength(50)]
        public string OrderId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a request type.")]
        public string RequestType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a reason.")]
        [StringLength(1000)]
        public string Reason { get; set; } = string.Empty;
    }
}
