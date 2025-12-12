using CarRentalWebApplication.Models;

namespace CarRentalWebApplication.Repositories.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCarsAsync(string sortBy, string searchQuery, bool onlyAvailable, bool includeHidden = false);
        Task<Car?> GetCarByIdAsync(int carId, bool includeHidden = false);
        Task AddCarAsync(Car car);
        Task HideCarAsync(int carId);
        Task UnHideCarAsync(int carId);
    }
}