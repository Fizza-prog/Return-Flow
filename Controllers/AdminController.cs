using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReturnFlow.Data;
using ReturnFlow.Models.ViewModels;

namespace ReturnFlow.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var recent = await _context.ReturnRequests
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .Take(5)
                .ToListAsync();

            var viewModel = new AdminDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalProducts = await _context.ReturnRequests
                    .Select(r => r.ProductName).Distinct().CountAsync(),
                TotalOrders = await _context.ReturnRequests
                    .Select(r => r.OrderId).Distinct().CountAsync(),
                TotalReturnRequests = await _context.ReturnRequests.CountAsync(),
                RecentActivity = recent.Select(r => new ActivityItem
                {
                    ActivityId = "ACT-" + r.Id.ToString("D3"),
                    UserName = r.User != null ? r.User.FullName : "Unknown",
                    Activity = r.Status == "Approved" ? "Return request approved"
                             : r.Status == "Rejected" ? "Return request rejected"
                             : "Submitted a return request",
                    Module = "Returns",
                    Date = r.CreatedAt,
                    Status = r.Status
                }).ToList()
            };

            return View(viewModel);
        }
    }
}
