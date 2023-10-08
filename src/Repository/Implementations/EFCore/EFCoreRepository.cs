using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Repository.Implementations.MySql;
using Repository.Interfaces;

namespace Repository.Implementations.EFCore
{
    public class EFCoreRepository<T> : IRepository<T> where T : class
    {
        private MySqlDbContext _context;
        private DbSet<T> _dbSet;
        private CancellationToken _cancellationToken;
        private IValidator<T> _validator;

        public EFCoreRepository(string connectionString, IValidator<T> validator, CancellationToken cancellationToken = default)
        {
            _context = new MySqlDbContext(connectionString);
            _cancellationToken = cancellationToken;
            _validator = validator;
            _dbSet = _context.Set<T>();
        }


        public IQueryable<T> Entites => _dbSet.AsQueryable();

        public async Task DeleteAsync(T entity)
        {
            _validator.ValidateAndThrow(entity);

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(_cancellationToken);
        }

        public async Task InsertAsync(T entity)
        {
            _validator.ValidateAndThrow(entity);

            _dbSet.Add(entity);
            await _context.SaveChangesAsync(_cancellationToken);
        }

        public async Task UpdateAsync(T entity)
        {
            _validator.ValidateAndThrow(entity);

            _dbSet.Update(entity);

            await _context.SaveChangesAsync(_cancellationToken);
        }
    }
}
