using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [Range(0, int.MaxValue)]
        public int Price { get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        [Required]
        [MaxLength(45)]
        public string Category { get; set; }
        public string Shelf { get; set; }
        [Range(0, 1000)]
        public int Count { get; set; }
        public string Description { get; set; }
    }
}
