using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Test.Modal.Interfaces;

namespace Test.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression, List<string> includeProperty = null);

        Task<TEntity> Get(int id, List<string> includeProperty = null);

        Task<TEntity> Add(TEntity entity);

        Task<TEntity> Update(TEntity entity);

        Task<TEntity> Delete(int id);

        Task<int> Count(Expression<Func<TEntity, bool>> expression = null);
    }
}
