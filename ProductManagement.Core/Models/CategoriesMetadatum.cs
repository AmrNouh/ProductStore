﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ProductManagement.Core.Models
{
    public partial class CategoriesMetadatum
    {
        public CategoriesMetadatum()
        {
            ProductsMetadata = new HashSet<ProductsMetadatum>();
        }

        public int MetadataId { get; set; }
        public int CatId { get; set; }
        public string MetadataText { get; set; }

        public virtual Category Cat { get; set; }
        public virtual ICollection<ProductsMetadatum> ProductsMetadata { get; set; }
    }
}