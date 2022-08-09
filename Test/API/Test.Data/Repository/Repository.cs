 using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Test.Data.Context;
using Test.Data.UnitOfWork;
using Test.Modal.Interfaces;

namespace Test.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly TestDbContext _context;

        public Repository(IUnitOfWork unitOfWork)
        {
            _context = unitOfWork.Context;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _ = await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null) return null;
            _context.Set<TEntity>().Remove(entity);
            _ = await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }

        public async Task<TEntity> Get(int id, List<string> includeProperty = null)
        {
            var query = _context.Set<TEntity>().Where(x => x.Id == id);
            if (includeProperty != null)
                query = includeProperty.Aggregate(query, (current, prop) => current.Include(prop));

            return await _context.Set<TEntity>().FindAsync(id);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression, List<string> includeProperty = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (expression != null) query = query.Where(expression);
            if (includeProperty != null)
                query = includeProperty.Aggregate(query, (current, prop) => current.Include(prop));
            return query;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>> expression = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (expression != null) query = query.Where(expression);
            return await query.CountAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _ = await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }
    }
}
