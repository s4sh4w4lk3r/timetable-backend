namespace Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Entites { get; }
        Task InsertAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
