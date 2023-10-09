using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Repository.Implementations.MySql;
using Repository.Interfaces;

namespace Repository.Implementations.EFCore
{
    public class EFCoreRepository<T> : IRepository<T> where T : class
    {
        private readonly SqlDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IValidator<T> _validator;

        public EFCoreRepository(string connectionString, IValidator<T> validator)
        {
            _context = new SqlDbContext(connectionString);
            _validator = validator;
            _dbSet = _context.Set<T>();
        }


        public IQueryable<T> Entites => _dbSet.AsQueryable();

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _validator.ValidateAndThrow(entity);

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            _validator.ValidateAndThrow(entity);

            _dbSet.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _validator.ValidateAndThrow(entity);

            _dbSet.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
