
using System;
using System.Threading.Tasks;

namespace ProductManagement.Core.Repositories.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IProductsMetadataRepository ProductsMetadata { get; }
        ICategoriesMetadataRepository CategoriesMetadata { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}
