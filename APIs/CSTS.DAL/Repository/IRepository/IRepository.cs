using CSTS.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRepository<T> where T : class
{
    Task<WebResponse<IEnumerable<T>>> GetAllAsync();
    Task<WebResponse<T>> GetByIdAsync(Guid id);
    Task<WebResponse<bool>> AddAsync(T entity);
    Task<WebResponse<bool>> UpdateAsync(T entity);
    Task<WebResponse<bool>> DeleteAsync(Guid id);
    Task<WebResponse<IEnumerable<T>>> FindAsync(Func<T, bool> predicate);
}
