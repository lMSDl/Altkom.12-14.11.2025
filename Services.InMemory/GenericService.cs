using Bogus;
using Microsoft.Extensions.Options;
using Services.Interfaces;

namespace Services.InMemory
{
    public class GenericService<T> : IGenericService<T> where T : Models.Entity
    {
        protected readonly List<T> _entities;
        public GenericService() { 
            _entities = [];
        }

        public GenericService(Faker<T> faker, int numberOfItems)
        {
            _entities = faker.Generate(numberOfItems);
        }

        public GenericService(Faker<T> faker, IOptions<Models.Settings.Bogus> options)
        {
            _entities = faker.Generate(options.Value.NumberOfResources);
        }

        public Task<int> CreateAsync(T entity)
        {
            entity.Id = _entities.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            _entities.Add(entity);
            return Task.FromResult(entity.Id);
        }

        public Task DeleteAsync(int id)
        {
            var entity = _entities.SingleOrDefault(x => x.Id == id);
            if (entity is null)
                throw new KeyNotFoundException($"Entity with Id {id} not found");
            _entities.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<T>> ReadAsync()
        {
            return Task.FromResult(_entities.AsEnumerable());
        }

        public Task<IEnumerable<T>> ReadAsync(Func<T, bool> action)
        {
            return Task.FromResult(_entities.Where(action).AsEnumerable());
        }

        public Task<T?> ReadByIdAsync(int id)
        {
            var entity = _entities.SingleOrDefault(x => x.Id == id);
            return Task.FromResult(entity);
        }

        public Task UpdateAsync(int id, T entity)
        {
            var index = _entities.ToList().FindIndex(x => x.Id == id);
            if (index == -1)
                throw new KeyNotFoundException($"Entity with Id {id} not found");
            entity.Id = id;
            _entities[index] = entity;
            return Task.CompletedTask;
        }
    }
}
