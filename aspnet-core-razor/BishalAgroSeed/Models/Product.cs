using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BishalAgroSeed.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter the product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter the product weight")]
        [DataType("decimal(16, 2)")]
        public int Weight { get; set; }
        [Required(ErrorMessage = "Enter the product price")]
        [DataType("decimal(16, 2)")]
        public int Price { get; set; }
        public string Brand { get; set; }
        public string ImagePath { get; set; }
    }
}
