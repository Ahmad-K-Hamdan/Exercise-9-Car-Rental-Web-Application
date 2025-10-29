using Microsoft.AspNetCore.Mvc;

namespace CarRentalWebApplication.Controllers
{
    public class BookingHistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
