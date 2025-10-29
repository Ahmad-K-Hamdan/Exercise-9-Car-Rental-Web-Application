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

        public async Task<IEnumerable<Car>> GetAllCarsAsync(string sortBy, bool onlyAvailable)
        {
            IQueryable<Car> query = _applicationDbContext.Cars;

            if (onlyAvailable)
            {
                query = query.Where(c => c.Available == true);
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

        public async Task<IEnumerable<Car>> SearchCarsAsync(string searchQuery) => await _applicationDbContext.Cars
            .Where(c =>
                c.Make.Contains(searchQuery) ||
                c.Model.Contains(searchQuery) ||
                c.Color!.Contains(searchQuery) ||
                c.Description!.Contains(searchQuery))
            .ToListAsync();
    }
}
