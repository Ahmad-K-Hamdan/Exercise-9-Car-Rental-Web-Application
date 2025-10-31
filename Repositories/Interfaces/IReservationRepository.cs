using CarRentalWebApplication.Models;

namespace CarRentalWebApplication.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        Task<Rental> CreateAsync(Rental rental);
        Task<IEnumerable<Rental>> GetAllAsync();
        Task<Rental?> GetByIdAsync(int rentalId);
        Task<bool> ReturnCarAsync(int rentalId);
    }
}