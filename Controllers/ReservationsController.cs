using CarRentalWebApplication.Data;
using CarRentalWebApplication.Models;
using CarRentalWebApplication.Repositories;
using CarRentalWebApplication.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarRentalWebApplication.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICarRepository _carRepository;

        public ReservationsController(IReservationRepository reservationRepository, ICarRepository carRepository)
        {
            _reservationRepository = reservationRepository;
            _carRepository = carRepository;
        }

        public async Task<IActionResult> Create(int carId)
        {
            var car = await _carRepository.GetCarByIdAsync(carId);

            if (car == null)
            {
                throw new KeyNotFoundException("Car not found.");
            }

            if (!car.Available)
            {
                throw new InvalidOperationException("Car is not available for reservation.");
            }

            var rental = new Rental
            {
                CarId = carId,
                Car = car,
                StartDate = DateTime.Today,
                StartTime = DateTime.Now.TimeOfDay
            };

            return View(rental);
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedAccessException("You must be logged in to reserve a car.");
            }

            var rentals = await _reservationRepository.GetByUserAsync(userId, true);
            return View(rentals);
        }

        public async Task<IActionResult> History()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedAccessException("You must be logged in to reserve a car.");
            }

            var rentals = await _reservationRepository.GetByUserAsync(userId, false);
            return View(rentals);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Rental rental, DateTime returnDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedAccessException("You must be logged in to reserve a car.");
            }

            if (returnDate < rental.StartDate)
            {
                throw new ArgumentException("Return date must be after or equal to the start date.");
            }

            rental.UserId = userId;
            rental.ReturnDate = returnDate;
            rental.DurationDays = (int)(returnDate - rental.StartDate).TotalDays + 1;
            var createdRental = await _reservationRepository.CreateAsync(rental);

            return RedirectToAction("Confirmation", new { id = createdRental.RentalId });
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var rental = await _reservationRepository.GetByIdAsync(id);

            if (rental == null)
            {
                throw new KeyNotFoundException("Reservation not found.");
            }

            return View(rental);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            await _reservationRepository.ReturnCarAsync(id);
            return RedirectToAction("Index", "Cars");
        }
    }
}
