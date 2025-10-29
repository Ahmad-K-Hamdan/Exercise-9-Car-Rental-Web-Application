using Microsoft.AspNetCore.Mvc;

namespace CarRentalWebApplication.Controllers
{
    public class CarsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
