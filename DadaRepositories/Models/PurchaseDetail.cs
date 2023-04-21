using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class PurchaseDetail
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int PurchaseId { get; set; }

        [Required]
        public int Line { get; set; }

        [Required]
        public string Code { get; set; }

        public string Description { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public double Quantity { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public double UnitCost { get; set; }

        public double Cost { get => Quantity * UnitCost; }
    }
}
