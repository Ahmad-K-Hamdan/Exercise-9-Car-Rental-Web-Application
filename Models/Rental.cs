using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalWebApplication.Models
{
    public class Rental
    {
        [Key]
        public int RentalId { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public Car Car { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [StringLength(30)]
        public string PaymentType { get; set; } = null!;

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        [StringLength(255)]
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsReturned { get; set; } = false;
    }
}
