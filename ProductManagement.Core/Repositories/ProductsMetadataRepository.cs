using ProductManagement.Core.Models;
using ProductManagement.Core.Repositories.IRepositories;

namespace ProductManagement.Core.Repositories
{
    public class ProductsMetadataRepository : BaseRepository<ProductsMetadatum>, IProductsMetadataRepository
    {
        public ProductsMetadataRepository(ProductStoreDBContext productManagementDb) : base(productManagementDb)
        {

        }
    }
}
