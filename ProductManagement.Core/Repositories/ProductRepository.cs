using Microsoft.EntityFrameworkCore;
using ProductManagement.Core.Models;
using ProductManagement.Core.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagement.Core.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ProductStoreDBContext _productManagementDb;

        public ProductRepository(ProductStoreDBContext productManagementDb) : base(productManagementDb)
        {
            _productManagementDb = productManagementDb;
        }

        public Product GetProductByName(string name)
        {
            return _productManagementDb.Products.SingleOrDefault(p => p.Name == name);
        }

        public IEnumerable<Metadata> GetProductMetadata(int productId)
        {
            return _productManagementDb.ProductsMetadata.Include(pm => pm.Metadata).Where(pm => pm.ProdId == productId).Select(pm => new Metadata { Id = pm.Metadata.MetadataId, Text = pm.Metadata.MetadataText }).ToList();

        }

        public List<Product> GetProductsByDate(DateTime? createdDate)
        {
            return createdDate is null ? new List<Product>() : _productManagementDb.Products.Where(p => p.CreatedAt.Equals(createdDate)).ToList();
        }

        public List<Product> GetProductsByMetadataText(string metadataText)
        {
            if (string.IsNullOrWhiteSpace(metadataText))
                return new List<Product>();

            var metadataList = _productManagementDb.CategoriesMetadata
                .Where(cm => cm.MetadataText == metadataText.Trim())
                .Select(cm => cm.MetadataId).ToList();

            return _productManagementDb.Products.Where(p => p.ProductsMetadata.Any(pm => metadataList.Contains(pm.MetadataId))).ToList();
        }

        public List<Product> GetProductsByName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                return new List<Product>();

            return _productManagementDb.Products
                    .Where(p => p.Name.Contains(productName.Trim())).ToList();
        }
    }
}
