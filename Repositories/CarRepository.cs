using CarRentalWebApplication.Data;
using CarRentalWebApplication.Models;
using CarRentalWebApplication.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalWebApplication.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CarRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync(string sortBy, string searchQuery, bool onlyAvailable)
        {
            IQueryable<Car> query = _applicationDbContext.Cars;

            if (onlyAvailable)
            {
                query = query.Where(c => c.Available == true);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                query = query.Where(c =>
                        c.Make.Contains(searchQuery) ||
                        c.Model.Contains(searchQuery) ||
                        c.Color != null && c.Color.Contains(searchQuery) ||
                        c.Description != null && c.Description.Contains(searchQuery));
            }

            query = sortBy?.ToLower() switch
            {
                "make" => query.OrderBy(c => c.Make),
                "model" => query.OrderBy(c => c.Model),
                "year" => query.OrderBy(c => c.Year),
                "rate" => query.OrderBy(c => c.DailyRate),
                _ => query.OrderBy(c => c.CarId)
            };

            return await query.ToListAsync();
        }

        public async Task<Car?> GetCarByIdAsync(int carId) => await _applicationDbContext.Cars.FirstOrDefaultAsync(c => c.CarId == carId);
    }
}
