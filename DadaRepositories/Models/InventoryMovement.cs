using DadaRepositories.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class InventoryMovement
    {
        public InventoryMovement()
        {
            Details = new List<InventoryMovementDetail>();
        }

        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy-MM-dd")]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        [Required, MustHaveOne(ErrorMessage = "At least one detail item is required")]
        public List<InventoryMovementDetail> Details { get; set; }
    }
}
