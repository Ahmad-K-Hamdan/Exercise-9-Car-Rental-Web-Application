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

        public async Task<IActionResult> Index(string sortBy, bool onlyAvailable = true)
        {
            return View(await _carRepository.GetAllCarsAsync(sortBy, onlyAvailable));
        }

        public async Task<IActionResult> Details(int id)
        {
            var car = await _carRepository.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }
    }
}
