using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Repository.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Repository.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _entities;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task<int> CreateAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }
        public async Task DeleteAsync(T entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id, params string[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            // Apply includes
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression, params string[] includes)
        {
            var query = _context.Set<T>().AsQueryable<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            T? entity = await query.FirstOrDefaultAsync(expression);
            return entity;
        }



        public IQueryable<T> GetFiltered(Expression<Func<T, bool>> expression, params string[] includes)
        {
            var query = _context.Set<T>().Where(expression).AsQueryable();
            foreach (string include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> expression, params string[] includes)
        {

            var query = _context.Set<T>().AsQueryable();
            foreach (string include in includes)
                query = query.Include(include);

            return await query.AnyAsync(expression);
        }

        public IQueryable<T> GetAll(params string[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

    }
}
