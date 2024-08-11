using CSTS.DAL.Enum;
using CSTS.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

    public IEnumerable<T> Get(int PageNumber = 1, int PageSize = 10)
    {
        return _dbSet.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
    }

    public IEnumerable<T> Get(int PageNumber = 1, int PageSize = 100, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet.AsQueryable();

        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);

        return query.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();

    }

    public IEnumerable<T> Get(out int count, int PageNumber = 1, int PageSize = 10)
    {
        count = _dbSet.Count();
        return _dbSet.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
    }

    public IEnumerable<T> Get(out int count, int PageNumber = 1, int PageSize = 100, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet.AsQueryable();

        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);

        count = query.Count();
        return query.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
    }




    public IEnumerable<T> Find(Func<T, bool> predicate, int PageNumber = 1, int PageSize = 10)
    {
        return _dbSet.Where((x) => predicate(x)).Skip((PageNumber - 1) * PageSize).Take(PageSize);
    }

    public IEnumerable<T> Find(Func<T, bool> predicate, int PageNumber = 1, int PageSize = 100, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet.AsQueryable();

        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);

        return query.Where((x) => predicate(x)).Skip((PageNumber - 1) * PageSize).Take(PageSize);
    }


    public IEnumerable<T> Find(out int count, Func<T, bool> predicate, int PageNumber = 1, int PageSize = 10)
    {
        var data = _dbSet
        .Where(x => predicate(x))
        .GroupBy(x => 1) // single group
        .Select(g => new
        {
            Count = g.Count(),
            Results = g.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList()
        })
        .FirstOrDefault();

        count = data?.Count ?? 0;
        return data?.Results ?? new List<T>();
    }

    public IEnumerable<T> Find(out int count, Func<T, bool> predicate, int PageNumber = 1, int PageSize = 100, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet.AsQueryable();

        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);

        var data = query
        .Where(x => predicate(x))
        .GroupBy(x => 1) // single group
        .Select(g => new
        {
            Count = g.Count(),
            Results = g.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList()
        })
        .FirstOrDefault();

        count = data?.Count ?? 0;
        return data?.Results ?? new List<T>();

    }



    public T GetById(Guid Id)
    {
        return _dbSet.Find(Id);
    }

    public bool Add(T entity)
    {
        _dbSet.Add(entity);
        return _context.SaveChanges() > 0;
    }

    public bool Update(T entity)
    {
        _dbSet.Update(entity);
        return _context.SaveChanges() > 0;

    }

    public bool Delete(Guid id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null)
            return false;
        else
            _dbSet.Remove(entity);

        return _context.SaveChanges() > 0;
    }


    public int Count(Func<T, bool> predicate)
    {
        return _dbSet.Count(c => predicate(c));
    }

    public IQueryable<T> GetQueryable()
    {
        return _dbSet.AsQueryable();
    }

}

