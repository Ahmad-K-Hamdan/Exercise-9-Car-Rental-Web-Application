using CarRentalWebApplication.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalWebApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminReservationsController : Controller
    {
        private readonly IReservationRepository _reservationRepository;

        public AdminReservationsController(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<IActionResult> Index(string searchQuery, string statusFilter)
        {
            var reservations = await _reservationRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                reservations = reservations.Where(r =>
                    r.Car.Make.ToLower().Contains(searchQuery) ||
                    r.Car.Model.ToLower().Contains(searchQuery) ||
                    r.User.FirstName.ToLower().Contains(searchQuery) ||
                    r.User.LastName.ToLower().Contains(searchQuery) ||
                    r.User.Email.ToLower().Contains(searchQuery) ||
                    r.RentalId.ToString().Contains(searchQuery)
                ).ToList();
            }

            statusFilter = statusFilter?.ToLower() ?? "all";
            reservations = statusFilter switch
            {
                "active" => reservations.Where(r => !r.IsReturned && DateTime.Today <= r.ReturnDate).ToList(),
                "overdue" => reservations.Where(r => !r.IsReturned && DateTime.Today > r.ReturnDate).ToList(),
                "completed" => reservations.Where(r => r.IsReturned).ToList(),
                _ => reservations
            };

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ReservationTable", reservations);
            }

            ViewData["SearchQuery"] = searchQuery;
            ViewData["StatusFilter"] = statusFilter;
            return View(reservations);
        }
    }
}
