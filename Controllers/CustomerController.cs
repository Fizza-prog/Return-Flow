
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReturnFlow.Data;
using ReturnFlow.Models;
using ReturnFlow.Models.ViewModels;

namespace ReturnFlow.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Challenge();
            }

            var requests = await _context.ReturnRequests
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            var viewModel = new CustomerDashboardViewModel
            {
                ReturnRequests = requests,
                TotalRequests = requests.Count,
                PendingRequests = requests.Count(r => r.Status == "Pending"),
                ApprovedRequests = requests.Count(r => r.Status == "Approved")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReturnRequest(
            ReturnRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please complete all fields correctly.";
                return RedirectToAction(nameof(Dashboard));
            }

            string[] allowedTypes = { "Refund", "Replacement", "Exchange" };

            if (!allowedTypes.Contains(model.RequestType))
            {
                TempData["Error"] = "Invalid request type selected.";
                return RedirectToAction(nameof(Dashboard));
            }

            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Challenge();
            }

            var request = new ReturnRequest
            {
                OrderId = model.OrderId,
                ProductName = model.ProductName,
                RequestType = model.RequestType,
                Reason = model.Reason,
                Status = "Pending",
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.ReturnRequests.Add(request);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Your return request has been submitted successfully.";

            return RedirectToAction(nameof(Dashboard));
        }
    }
}
