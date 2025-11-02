using CarRentalWebApplication.Models;
using CarRentalWebApplication.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalWebApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCarsController : Controller
    {
        private readonly ICarRepository _carRepository;

        public AdminCarsController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<IActionResult> Index(string sortBy, string searchQuery, string statusFilter = "all")
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

            return View(cars);
        }

        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Car car)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", car);
            }

            await _carRepository.AddCarAsync(car);
            TempData["SuccessMessage"] = "Car added successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Hide(int carId)
        {
            await _carRepository.HideCarAsync(carId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnHide(int carId)
        {
            await _carRepository.UnHideCarAsync(carId);
            return RedirectToAction(nameof(Index));
        }
    }
}
