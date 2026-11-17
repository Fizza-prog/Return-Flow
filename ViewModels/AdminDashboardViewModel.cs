namespace ReturnFlow.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }

        public int TotalProducts { get; set; }

        public int TotalOrders { get; set; }

        public int TotalReturnRequests { get; set; }

        public List<ActivityItem> RecentActivity { get; set; } = new();
    }

    public class ActivityItem
    {
        public string ActivityId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Activity { get; set; } = string.Empty;

        public string Module { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
