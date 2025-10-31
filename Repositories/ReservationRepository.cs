using CarRentalWebApplication.Data;
using CarRentalWebApplication.Exceptions;
using CarRentalWebApplication.Models;
using CarRentalWebApplication.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalWebApplication.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Rental?> GetByIdAsync(int rentalId)
        {
            return await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.RentalId == rentalId);
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            return await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetByUserAsync(string userId, bool activeStatus)
        {
            return await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .Where(r => r.IsActive == activeStatus)
                .ToListAsync();
        }

        public async Task<Rental> CreateAsync(Rental rental)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == rental.CarId);

            if (car == null)
            {
                throw new NotFoundException("Car not found.");
            }

            if (!car.Available)
            {
                throw new InvalidOperationException("Car is not available for reservation.");
            }

            rental.DurationDays = rental.DurationDays;
            rental.TotalPrice = rental.DurationDays * car.DailyRate;
            await _context.Rentals.AddAsync(rental);

            car.Available = false;
            _context.Cars.Update(car);

            await _context.SaveChangesAsync();
            return rental;
        }

        public async Task<bool> ReturnCarAsync(int rentalId)
        {
            var rental = await _context.Rentals.Include(r => r.Car).FirstOrDefaultAsync(r => r.RentalId == rentalId);

            if (rental == null)
            {
                throw new NotFoundException("Rental not found.");
            }

            rental.IsReturned = true;
            rental.IsActive = false;
            rental.ActualReturnDate = DateTime.Today;
            rental.Car.Available = true;

            _context.Update(rental);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
