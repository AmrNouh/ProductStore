using Microsoft.AspNetCore.Identity;
using ProductManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Core.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Core.Repositories
{
    public class RoleRepository : IIdentityBase<Role>
    {
        private readonly ProductStoreDBContext _dbContext;

        public RoleRepository(ProductStoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<string> GetRoleIdAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(Role role)
        {
            throw new NotImplementedException();

        }

        public async Task SetRoleNameAsync(Role role)
        {
            var oldRole = await  _dbContext.Roles.FindAsync(role.RoleId);

            if (oldRole is not null)
            {
                oldRole.Name = role.Name;
            }
            await _dbContext.SaveChangesAsync();

        }

        public async Task<IdentityResult> CreateAsync(Role entity)
        {
            _dbContext.Roles.Add(entity);
            var rows = await _dbContext.SaveChangesAsync();
            return rows > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = $"Could not insert Role {entity.Name}." });
        }

        public async Task<IdentityResult> UpdateAsync(Role entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            var rows = await _dbContext.SaveChangesAsync();
            return rows > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = $"Could not Update Role {entity.Name}." });
        }

        public async Task<IdentityResult> DeleteAsync(Role entity)
        {
            var role = await _dbContext.Roles.FindAsync(entity.RoleId);

            if (role is null)
                return IdentityResult.Failed(new IdentityError { Description = $"Could not Find Role {entity.Name}." });
           
            _dbContext.Roles.Remove(role);
            var rows = await _dbContext.SaveChangesAsync();
            return rows > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = $"Could not Delete Role {entity.Name}." });
        }

        public async Task<Role> FindByIdAsync(string entityId)
        {
            return await _dbContext.Roles.FindAsync(entityId);
        }

        public async Task<Role> FindByNameAsync(string normalizedEntityName)
        {
            return await _dbContext.Roles.SingleOrDefaultAsync(role => role.Name == normalizedEntityName);
        }
    }
}
