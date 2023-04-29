using DadaRepositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class InvoiceDetail : IBaseFirestoreData
    {
        public string Id { get; set; }

        public string WorkOrderId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public int Quantity { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public double UnitPrice { get; set; }

        public double Price { get => Quantity * UnitPrice; }
    }
}
