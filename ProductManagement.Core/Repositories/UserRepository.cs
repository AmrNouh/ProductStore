using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Core.Models;
using ProductManagement.Core.Repositories.IRepositories;

namespace ProductManagement.Core.Repositories
{
    public class UserRepository : IIdentityBase<User>
    {
        private readonly ProductStoreDBContext _dbContext;

        public UserRepository(ProductStoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IdentityResult> CreateAsync(User entity)
        {
            throw new NotImplementedException();

        }

        public Task<IdentityResult> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string entityId)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByNameAsync(string normalizedEntityName)
        {
            throw new NotImplementedException();
        }
    }
}
