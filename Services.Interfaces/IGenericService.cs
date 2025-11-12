namespace Services.Interfaces
{
    public interface IGenericService<T>
    {
        Task<IEnumerable<T>> ReadAsync();
        Task<IEnumerable<T>> ReadAsync(Func<T, bool> action);
        Task<T?> ReadByIdAsync(int id);
        Task<int> CreateAsync(T entity);
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
    }
}
