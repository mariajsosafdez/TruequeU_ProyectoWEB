using Microsoft.AspNetCore.Mvc;

namespace TruequeU.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
