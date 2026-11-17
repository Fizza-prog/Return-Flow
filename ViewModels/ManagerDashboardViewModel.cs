using ReturnFlow.Models;

namespace ReturnFlow.Models.ViewModels
{
    public class ManagerDashboardViewModel
    {
        public List<ReturnRequest> ReturnRequests { get; set; } = new();

        public int TotalRequests { get; set; }

        public int PendingRequests { get; set; }

        public int ApprovedRequests { get; set; }

        public int RejectedRequests { get; set; }
    }
}
