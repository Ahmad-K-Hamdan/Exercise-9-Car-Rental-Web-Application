using CarRentalWebApplication.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalWebApplication.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarRepository _carRepository;

        public CarsController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<IActionResult> Index(string sortBy, string searchQuery, bool onlyAvailable = true)
        {
            var cars = await _carRepository.GetAllCarsAsync(sortBy, searchQuery, true);

            ViewData["CurrentSort"] = sortBy;
            ViewData["SearchQuery"] = searchQuery;

            if (HttpContext.Request.Headers.XRequestedWith == "XMLHttpRequest")
            {
                return View("_CarCard", cars);
            }

            return View(cars);
        }

        public async Task<IActionResult> Details(int carId)
        {
            var car = await _carRepository.GetCarByIdAsync(carId);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }
    }
}
