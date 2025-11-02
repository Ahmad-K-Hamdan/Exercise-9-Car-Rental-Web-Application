using CarRentalWebApplication.Exceptions;
using CarRentalWebApplication.Models;
using CarRentalWebApplication.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalWebApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IReservationRepository _reservationRepository;
        private readonly ICarRepository _carRepository;

        public AdminUsersController(UserManager<User> userManager, IReservationRepository reservationRepository, ICarRepository carRepository)
        {
            _userManager = userManager;
            _reservationRepository = reservationRepository;
            _carRepository = carRepository;
        }

        public async Task<IActionResult> Index(string searchQuery, string roleFilter)
        {
            var users = _userManager.Users.ToList();
            var userList = new List<(User user, IList<string> roles)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add((user, roles));
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                userList = userList
                    .Where(u => u.user.FirstName.ToLower().Contains(searchQuery)
                            || u.user.LastName.ToLower().Contains(searchQuery)
                            || u.user.Email.ToLower().Contains(searchQuery))
                    .ToList();
            }

            roleFilter = roleFilter?.ToLower();
            userList = roleFilter switch
            {
                "admin" => userList.Where(u => u.roles.Contains("Admin")).ToList(),
                "customer" => userList.Where(u => u.roles.Contains("Customer")).ToList(),
                _ => userList
            };

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_UserTable", userList);
            }

            ViewData["SearchQuery"] = searchQuery;
            ViewData["RoleFilter"] = roleFilter;

            return View(userList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PromoteToAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            await _userManager.AddToRoleAsync(user, "Admin");
            TempData["SuccessMessage"] = $"{user.FirstName} {user.LastName} has been promoted to Admin.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UserReservations(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var reservations = await _reservationRepository.GetAllAsync();
            var userReservations = reservations
                .Where(r => r.UserId == userId)
                .ToList();

            ViewData["FullName"] = $"{user.FirstName} {user.LastName}";
            return View("UserReservations", userReservations);
        }
    }
}
