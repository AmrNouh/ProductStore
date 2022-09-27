using ProductManagement.Core.Models;
using System;
using System.Collections.Generic;

namespace ProductManagement.API.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CategoryName { get; set; }
        public bool IsArchived { get; set; }
        public IEnumerable<Metadata> ProductMetadata { get; set; }
    }
}
