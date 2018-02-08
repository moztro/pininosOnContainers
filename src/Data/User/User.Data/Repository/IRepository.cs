using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace User.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(long id);
        Task<T> Get(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where, string order, bool descending, int take);
        Task<int> Insert(T entity);        
        Task<int> Update(T entity);
        Task<int> Delete(T entity);
        Task<int> Count(Expression<Func<T, bool>> where);
    }
}
