using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TruequeU.Interfaces;

namespace TruequeU.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ListingController : Controller
    {
        private readonly IListingService _listingService;
        public ListingController(IListingService listingService)
        {
            _listingService = listingService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
