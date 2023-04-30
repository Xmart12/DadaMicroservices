using DadaRepositories.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        [Required, MustHaveOne(ErrorMessage = "At least one detail item is required")]
        public List<InvoiceDetail> Details { get; set; }
    }
}
