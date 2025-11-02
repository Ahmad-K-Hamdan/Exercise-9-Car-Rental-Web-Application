using CarRentalWebApplication.Models;
using CarRentalWebApplication.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin(string sortBy, string searchQuery, string statusFilter = "all")
        {
            var cars = await _carRepository.GetAllCarsAsync(
                sortBy, searchQuery,
                statusFilter == "available",
                statusFilter == "hidden" || statusFilter == "all"
            );

            if (statusFilter == "rented")
            {
                cars = cars.Where(c => c.IsVisible && !c.IsAvailable);
            }
            else if (statusFilter == "hidden")
            {
                cars = cars.Where(c => !c.IsVisible);
            }

            ViewData["CurrentSort"] = sortBy;
            ViewData["SearchQuery"] = searchQuery;
            ViewData["StatusFilter"] = statusFilter;

            if (HttpContext.Request.Headers.XRequestedWith == "XMLHttpRequest")
            {
                return PartialView("_AdminCarTable", cars);
            }

            return View("Admin", cars);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(Car car)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", car);
            }

            await _carRepository.AddCarAsync(car);
            return RedirectToAction(nameof(Admin));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Hide(int carId)
        {
            await _carRepository.HideCarAsync(carId);
            return RedirectToAction(nameof(Admin));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnHide(int carId)
        {
            await _carRepository.UnHideCarAsync(carId);
            return RedirectToAction(nameof(Admin));
        }
    }
}
