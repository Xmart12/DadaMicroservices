using DadaRepositories.Attributes;
using DadaRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class Purchase : IBaseFirestoreData
    {
        public string Id { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy-MM-dd")]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        [Required, MustHaveOne(ErrorMessage = "At least one detail item is required")]
        public List<PurchaseDetail> Details { get; set; }
    }
}
