using DadaRepositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Models
{
    public class Product
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public double Cost { get; set; }

        [Required]
        public int Availability { get; set; }
    }
}
