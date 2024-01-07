using Microsoft.EntityFrameworkCore;

using Weather.Api.Core.Models;
using Weather.Api.Core.Repositories;

namespace Weather.Api.Persistence;

internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
   private readonly DbSet<TEntity> _entities;

   public Repository(DbContext dbContext)
   {
      _entities = dbContext.Set<TEntity>();
   }

   public async Task<TEntity?> GetAsync(int id)
   {
      // If an entity with the PK is being tracked by the context, it's returned without a request to the database.
      return await _entities.FindAsync(id);
   }

   public async Task AddAsync(TEntity entity)
   {
      await _entities.AddAsync(entity);
   }

   public void Remove(TEntity entity)
   {
      _entities.Remove(entity);
   }
}
