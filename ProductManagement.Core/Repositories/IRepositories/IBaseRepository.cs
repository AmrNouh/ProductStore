using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Core.Repositories.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        void Insert(T entity);
        Task InsertAsync(T entity);
        T Update(T entity);
        void Delete(int id);
        Task DeleteAsync(int id);
        void InsertRange(List<T> entities);
    }
}