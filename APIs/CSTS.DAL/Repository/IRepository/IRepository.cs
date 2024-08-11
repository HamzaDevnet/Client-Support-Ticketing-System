using CSTS.DAL.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public interface IRepository<T> where T : class
{
    Task<WebResponse<IEnumerable<T>>> GetAllAsync();
    Task<WebResponse<T>> GetByIdAsync(Guid id);
    Task<WebResponse<bool>> AddAsync(T entity);
    Task<WebResponse<bool>> UpdateAsync(T entity);
    Task<WebResponse<bool>> DeleteAsync(Guid id);
    Task<WebResponse<IEnumerable<T>>> FindAsync(Func<T, bool> predicate);
    Task<WebResponse<IEnumerable<T>>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
    Task<WebResponse<T>> GetIncludingAsync(Guid id, params Expression<Func<T, object>>[] includeProperties);
    IEnumerable<T> GetAll();
}
