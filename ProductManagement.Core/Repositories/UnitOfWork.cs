using ProductManagement.Core.Models;
using ProductManagement.Core.Repositories.IRepositories;
using System.Threading.Tasks;

namespace ProductManagement.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductStoreDBContext _dbContext;
        public IProductRepository Products { get; private set; }

        public ICategoryRepository Categories { get; private set; }

        public IProductsMetadataRepository ProductsMetadata { get; private set; }

        public ICategoriesMetadataRepository CategoriesMetadata { get; private set; }

        public UnitOfWork(ProductStoreDBContext dbContext, IProductRepository products, ICategoryRepository categories, IProductsMetadataRepository productsMetadata, ICategoriesMetadataRepository categoriesMetadata)
        {
            _dbContext = dbContext;
            Products = products;
            Categories = categories;
            ProductsMetadata = productsMetadata;
            CategoriesMetadata = categoriesMetadata;
        }



        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
