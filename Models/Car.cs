using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalWebApplication.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [Required]
        [StringLength(50)]
        public string Make { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = null!;

        public int Year { get; set; }

        [StringLength(30)]
        public string? Color { get; set; }

        [StringLength(20)]
        public string? Transmission { get; set; }

        [StringLength(20)]
        public string? FuelType { get; set; }

        [Required]
        [StringLength(20)]
        public string PlateNumber { get; set; } = null!;

        [Required]
        [Range(0, 99999999)]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal DailyRate { get; set; }

        public bool Available { get; set; } = true;

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public ICollection<Rental>? Rentals { get; set; }
    }
}
