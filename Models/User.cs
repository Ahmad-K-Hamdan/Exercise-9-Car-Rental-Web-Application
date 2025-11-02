using Microsoft.AspNetCore.Identity;

namespace CarRentalWebApplication.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string DriverLicenseNumber { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
    }
}