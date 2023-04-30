using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class InvoiceDetail
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        [Required]
        public string ProductId { get; set; }

        public Product Product { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public double Quantity { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public double UnitPrice { get; set; }

        public double Price { get => Quantity * UnitPrice; }
    }
}
