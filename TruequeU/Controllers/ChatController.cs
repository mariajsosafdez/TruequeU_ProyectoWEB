using Microsoft.AspNetCore.Mvc;

namespace TruequeU.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
