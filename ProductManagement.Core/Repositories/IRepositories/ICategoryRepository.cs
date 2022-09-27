using ProductManagement.Core.Models;
using System.Collections.Generic;

namespace ProductManagement.Core.Repositories.IRepositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Category GetCategoryByName(string name);
        IEnumerable<Metadata> GetCategoriesMetadata(int categoryId);
    }
}