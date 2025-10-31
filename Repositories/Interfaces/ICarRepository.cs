using CarRentalWebApplication.Models;

namespace CarRentalWebApplication.Repositories.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCarsAsync(string sortBy, string searchQuery, bool onlyAvailable);
        Task<Car?> GetCarByIdAsync(int CarId);
    }
}