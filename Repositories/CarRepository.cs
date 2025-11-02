using CarRentalWebApplication.Data;
using CarRentalWebApplication.Exceptions;
using CarRentalWebApplication.Models;
using CarRentalWebApplication.Repositories.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CarRentalWebApplication.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CarRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync(string sortBy, string searchQuery, bool onlyAvailable, bool includeHidden = false)
        {
            IQueryable<Car> query = _applicationDbContext.Cars;

            if (onlyAvailable)
            {
                query = query.Where(c => c.IsAvailable);
            }
            if (!includeHidden)
            {
                query = query.Where(c => c.IsVisible);
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

        public async Task<Car?> GetCarByIdAsync(int carId, bool includeHidden = false)
        {
            IQueryable<Car> query = _applicationDbContext.Cars;

            if (!includeHidden)
            {
                query = query.Where(c => c.IsVisible);
            }

            return await query.FirstOrDefaultAsync(c => c.CarId == carId);
        }

        public async Task AddCarAsync(Car car)
        {
            await _applicationDbContext.Cars.AddAsync(car);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task HideCarAsync(int carId)
        {
            var car = await GetCarByIdAsync(carId, true);
            if (car == null)
            {
                throw new NotFoundException("Car not found.");
            }

            car.IsVisible = false;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task UnHideCarAsync(int carId)
        {
            var car = await GetCarByIdAsync(carId, true);
            if (car == null)
            {
                throw new NotFoundException("Car not found.");
            }

            car.IsVisible = true;
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
