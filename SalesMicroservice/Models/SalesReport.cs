using DadaRepositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Utilities.Attributes;

namespace SalesMicroservice.Models
{
    public class SalesReport : IBaseFirestoreData
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public Customer Customer { get; set; }

        public string Description { get; set; }

        [Required, MustHaveOne(ErrorMessage = "At least one detail item is required")]
        public List<SalesReportDetail> Details { get; set; }
    }
}
