using DadaRepositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class InventoryMovementDetail : IBaseFirestoreData
    {
        public string Id { get; set; }

        public string InventoryMovementId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public int Quantity { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public double UnitCost { get; set; }

        public double Cost { get => Quantity * UnitCost; }
    }
}
