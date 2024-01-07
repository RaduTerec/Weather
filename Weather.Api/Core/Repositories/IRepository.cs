using Weather.Api.Core.Models;

namespace Weather.Api.Core.Repositories;

public interface IRepository<TEntity> where TEntity : IEntity
{
   Task<TEntity?> GetAsync(int id);
   Task AddAsync(TEntity entity);
   void Remove(TEntity entity);
}
