
using Microsoft.AspNetCore.Mvc;

namespace ReturnFlow.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
