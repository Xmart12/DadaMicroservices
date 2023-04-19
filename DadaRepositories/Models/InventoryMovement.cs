using DadaRepositories.Interfaces;
using DadaRepositories.Utilities;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Utilities.Attributes;

namespace DadaRepositories.Models
{
    public class InventoryMovement : IBaseFirestoreData
    {
        public string Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required, DataType(DataType.Date)]
        public string Date { get; set; }

        public string Description { get; set; }

        [Required, MustHaveOne(ErrorMessage = "At least one detail item is required")]
        public List<InventoryMovementDetail> Details { get; set; }
    }
}
