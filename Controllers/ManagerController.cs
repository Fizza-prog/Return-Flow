using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReturnFlow.Data;
using ReturnFlow.Models.ViewModels;

namespace ReturnFlow.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var requests = await _context.ReturnRequests
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            var viewModel = new ManagerDashboardViewModel
            {
                ReturnRequests = requests,
                TotalRequests = requests.Count,
                PendingRequests = requests.Count(r => r.Status == "Pending"),
                ApprovedRequests = requests.Count(r => r.Status == "Approved"),
                RejectedRequests = requests.Count(r => r.Status == "Rejected")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            if (status != "Approved" && status != "Rejected")
            {
                TempData["Error"] = "Invalid status.";
                return RedirectToAction(nameof(Dashboard));
            }

            var request = await _context.ReturnRequests.FindAsync(id);

            if (request == null)
            {
                TempData["Error"] = "Return request not found.";
                return RedirectToAction(nameof(Dashboard));
            }

            if (request.Status != "Pending")
            {
                TempData["Error"] = "Only pending requests can be approved or rejected.";
                return RedirectToAction(nameof(Dashboard));
            }

            request.Status = status;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Request RET-{request.Id:D3} was {status.ToLower()}.";
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inspect(InspectionViewModel model)
        {
            string[] conditions = { "Excellent", "Good", "Damaged", "Defective" };
            string[] dispositions = { "Restock", "Repair", "Replace", "Discard" };

            if (!ModelState.IsValid
                || !conditions.Contains(model.Condition)
                || !dispositions.Contains(model.Disposition))
            {
                TempData["Error"] = "Please complete all inspection fields correctly.";
                return RedirectToAction(nameof(Dashboard));
            }

            // Accepts "RET-001" or just "1"
            var idText = model.RequestId.Trim();

            if (idText.StartsWith("RET-", StringComparison.OrdinalIgnoreCase))
            {
                idText = idText.Substring(4);
            }

            if (!int.TryParse(idText, out var id))
            {
                TempData["Error"] = "Invalid request ID. Use a format like RET-001.";
                return RedirectToAction(nameof(Dashboard));
            }

            var request = await _context.ReturnRequests.FindAsync(id);

            if (request == null)
            {
                TempData["Error"] = $"Return request {model.RequestId} was not found.";
                return RedirectToAction(nameof(Dashboard));
            }

            request.ProductCondition = model.Condition;
            request.InspectionFindings = model.Findings;
            request.Disposition = model.Disposition;
            request.InspectedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Inspection saved for RET-{request.Id:D3}.";
            return RedirectToAction(nameof(Dashboard));
        }
    }
}
