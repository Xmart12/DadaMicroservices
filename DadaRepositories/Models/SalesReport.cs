using DadaRepositories.Attributes;
using DadaRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class SalesReport
    {
        public SalesReport()
        {
            Details = new List<SalesReportDetail>();
        }

        public int Id { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string CustomerId { get; set; }

        public Customer Customer { get; set; }

        public string Description { get; set; }

        [Required, MustHaveOne(ErrorMessage = "At least one detail item is required")]
        public List<SalesReportDetail> Details { get; set; }
    }
}
