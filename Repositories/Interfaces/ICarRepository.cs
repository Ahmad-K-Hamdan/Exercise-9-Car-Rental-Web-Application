using CarRentalWebApplication.Models;

namespace CarRentalWebApplication.Repositories.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCarsAsync(string sortBy, bool onlyAvailable);
        Task<Car?> GetCarByIdAsync(int CarId);
        Task<IEnumerable<Car>> SearchCarsAsync(string searchQuery);
    }
}