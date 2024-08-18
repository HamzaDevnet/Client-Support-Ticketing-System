using CSTS.DAL.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public interface IRepository<T> where T : class
{
    IEnumerable<T> Get(int PageNumber = 1, int PageSize = 100);
    IEnumerable<T> Get(int PageNumber = 1, int PageSize = 100, params Expression<Func<T, object>>[] includeProperties);
    public IEnumerable<T> Get(out int count, int PageNumber = 1, int PageSize = 100);
    public IEnumerable<T> Get(out int count, int PageNumber = 1, int PageSize = 100, params Expression<Func<T, object>>[] includeProperties);


    IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int PageNumber = 1, int PageSize = 100);
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int PageNumber = 1, int PageSize = 100, params Expression<Func<T, object>>[] includeProperties);
    public IEnumerable<T> Find(out int count, Func<T, bool> predicate, int PageNumber = 1, int PageSize = 100);
    public IEnumerable<T> Find(out int count, Func<T, bool> predicate, int PageNumber = 1, int PageSize = 100, params Expression<Func<T, object>>[] includeProperties);

    T GetById(Guid id);

    bool Add(T entity);
    bool Update(T entity);
    bool Delete(Guid id);

    int Count(Func<T, bool> predicate);
    IQueryable<T> GetQueryable();

}
