using ProductManagement.Core.Models;
using ProductManagement.Core.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagement.Core.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly ProductStoreDBContext _productManagementDb;

        public CategoryRepository(ProductStoreDBContext productManagementDb) : base(productManagementDb)
        {
            _productManagementDb = productManagementDb;
        }

        public IEnumerable<Metadata> GetCategoriesMetadata(int categoryId)
        {
            return _productManagementDb.CategoriesMetadata.Where(cm => cm.CatId == categoryId).Select(cm => new Metadata { Id = cm.MetadataId, Text = cm.MetadataText }).ToList();
        }

        public Category GetCategoryByName(string name)
        {
            return _productManagementDb.Categories.SingleOrDefault(c => c.Name == name);
        }
    }
}
