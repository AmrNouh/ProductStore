using ProductManagement.Core.Models;
using System;
using System.Collections.Generic;

namespace ProductManagement.Core.Repositories.IRepositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Product GetProductByName(string name);

        List<Product> GetProductsByName(string productName);

        List<Product> GetProductsByDate(DateTime? createdDate);

        List<Product> GetProductsByMetadataText(string metadataText);

        IEnumerable<Metadata> GetProductMetadata(int productId);
    }
}