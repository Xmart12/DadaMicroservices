using DadaRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class WorkOrder : IBaseFirestoreData
    {
        public string Id { get; set; }

        [Required]
        public string ClientDocument { get; set; }

        public Customer Customer { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public List<WorkOrderDetail> Details { get; set; }

    }
}
