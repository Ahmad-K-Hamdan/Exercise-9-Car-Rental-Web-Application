using Microsoft.AspNetCore.Mvc;

namespace CarRentalWebApplication.Controllers
{
    public class ReservationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
