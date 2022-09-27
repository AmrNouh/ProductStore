using ProductManagement.Core.Models;
using ProductManagement.Core.Repositories.IRepositories;

namespace ProductManagement.Core.Repositories
{
    public class CategoriesMetadataRepository : BaseRepository<CategoriesMetadatum>, ICategoriesMetadataRepository
    {
        public CategoriesMetadataRepository(ProductStoreDBContext productManagementDb) : base(productManagementDb)
        {

        }
    }
}
