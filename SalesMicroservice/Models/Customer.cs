﻿using System.ComponentModel.DataAnnotations;

namespace SalesMicroservice.Models
{
    public class Customer
    {
        [Required]
        public int Id { get; set; }

        [Required, MinLength(8, ErrorMessage = "Document is not valid. Min. 8")]
        public string Document { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
