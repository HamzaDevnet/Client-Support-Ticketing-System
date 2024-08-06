using CSTS.DAL.Enum;
using CSTS.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<WebResponse<IEnumerable<T>>> GetAllAsync()
    {
        var data = await _dbSet.ToListAsync();
        return new WebResponse<IEnumerable<T>>()
        {
            Data = data,
            Code = ResponseCode.Success,
            Message = ResponseCode.Success.ToString()
        };
    }

    public async Task<WebResponse<T>> GetByIdAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            return new WebResponse<T>()
            {
                Data = null,
                Code = ResponseCode.Null,
                Message = "Entity not found."
            };
        }

        return new WebResponse<T>()
        {
            Data = entity,
            Code = ResponseCode.Success,
            Message = ResponseCode.Success.ToString()
        };
    }

    public async Task<WebResponse<bool>> AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return new WebResponse<bool>()
            {
                Data = result > 0,
                Code = result > 0 ? ResponseCode.Success : ResponseCode.Error,
                Message = result > 0 ? ResponseCode.Success.ToString() : ResponseCode.Error.ToString()
            };
        }
        catch (Exception ex)
        {
            return new WebResponse<bool>()
            {
                Data = false,
                Code = ResponseCode.Error,
                Message = ex.Message
            };
        }
    }

    public async Task<WebResponse<bool>> UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            var result = await _context.SaveChangesAsync();
            return new WebResponse<bool>()
            {
                Data = result > 0,
                Code = result > 0 ? ResponseCode.Success : ResponseCode.Error,
                Message = result > 0 ? ResponseCode.Success.ToString() : ResponseCode.Error.ToString()
            };
        }
        catch (Exception ex)
        {
            return new WebResponse<bool>()
            {
                Data = false,
                Code = ResponseCode.Error,
                Message = ex.Message
            };
        }
    }

    public async Task<WebResponse<bool>> DeleteAsync(Guid id)
    {
        try
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return new WebResponse<bool>()
                {
                    Data = false,
                    Code = ResponseCode.Error,
                    Message = "Entity not found."
                };
            }

            _dbSet.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return new WebResponse<bool>()
            {
                Data = result > 0,
                Code = result > 0 ? ResponseCode.Success : ResponseCode.Error,
                Message = result > 0 ? ResponseCode.Success.ToString() : ResponseCode.Error.ToString()
            };
        }
        catch (Exception ex)
        {
            return new WebResponse<bool>()
            {
                Data = false,
                Code = ResponseCode.Error,
                Message = ex.Message
            };
        }
    }

    public async Task<WebResponse<IEnumerable<T>>> FindAsync(Func<T, bool> predicate)
    {
        var data = await Task.FromResult(_dbSet.Where(predicate).ToList());
        return new WebResponse<IEnumerable<T>>()
        {
            Data = data,
            Code = ResponseCode.Success,
            Message = ResponseCode.Success.ToString()
        };
    }
}
