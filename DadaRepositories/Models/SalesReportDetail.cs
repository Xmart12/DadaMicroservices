using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class SalesReportDetail
    {
        public int Id { get; set; }

        [Required]
        public int SalesReportId { get; set; }

        [Required]
        public int Line { get; set; }

        [Required]
        public string ProductId { get; set; }

        public Product Product { get; set; }

        public string Description { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public double Quantity { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public double UnitCost { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public double UnitPrice { get; set; }

        public double Cost { get => Quantity * UnitCost; }

        public double Price { get => Quantity * UnitPrice; }
    }
}
