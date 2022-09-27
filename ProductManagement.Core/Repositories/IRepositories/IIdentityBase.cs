using Microsoft.AspNetCore.Identity;
using ProductManagement.Core.Models;
using System.Threading.Tasks;

namespace ProductManagement.Core.Repositories.IRepositories
{
    public interface IIdentityBase<T> where T : class
    {
        Task<IdentityResult> CreateAsync(T entity);
        Task<IdentityResult> UpdateAsync(T entity);
        Task<IdentityResult> DeleteAsync(T entity);
        Task<T> FindByIdAsync(string entityId);
        Task<T> FindByNameAsync(string normalizedEntityName);
    }
}