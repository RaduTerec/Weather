using System.Collections;

using Weather.Api.Core.Models;
using Weather.Api.Core.Repositories;

namespace Weather.Unit.Tests.Common.Fakes;

internal class FakeRepository<T> : IRepository<T>, IEnumerable<T>
   where T : IEntity
{
   private readonly Dictionary<int, T> _inMemoryRepository = [];

   public Task AddAsync(T entity)
   {
      _inMemoryRepository[entity.Id] = entity;
      return Task.CompletedTask;
   }

   public Task<T?> GetAsync(int id)
   {
      if (_inMemoryRepository.TryGetValue(id, out var entity))
      {
         return Task.FromResult<T?>(entity);
      }

      return Task.FromResult<T?>(default);
   }

   public void Remove(T entity) => _inMemoryRepository.Remove(entity.Id);

   public IEnumerator<T> GetEnumerator() => _inMemoryRepository.Values.GetEnumerator();

   IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
