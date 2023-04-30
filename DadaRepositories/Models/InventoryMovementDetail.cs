using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class InventoryMovementDetail
    {
        public int Id { get; set; }

        [Required]
        public int InventoryMovementId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public int Quantity { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public double UnitCost { get; set; }

        public double Cost { get => Quantity * UnitCost; }
    }
}
