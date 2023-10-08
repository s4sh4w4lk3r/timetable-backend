namespace Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Entites { get; }
        Task InsertAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    }
}
