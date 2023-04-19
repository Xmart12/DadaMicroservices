using DadaRepositories.Attributes;
using DadaRepositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class SalesReport : IBaseFirestoreData
    {
        public string Id { get; set; }

        public string CustomerDocument { get; set; }

        public Customer Customer { get; set; }

        public string Description { get; set; }

        [Required, MustHaveOne(ErrorMessage = "At least one detail item is required")]
        public List<SalesReportDetail> Details { get; set; }
    }
}
