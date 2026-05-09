using Microsoft.AspNetCore.Mvc;

namespace TruequeU.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
