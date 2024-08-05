using CSTS.DAL.Models;
using CSTS.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

   
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new Repository<User>(_context);
        Tickets = new Repository<Ticket>(_context);
        Comments = new Repository<Comment>(_context);
    }

    public IRepository<User> Users { get; private set; }
    public IRepository<Ticket> Tickets { get; private set; }
    public IRepository<Comment> Comments { get; private set; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
    public async Task<bool> CanConnectAsync()
    {
        return await _context.Database.CanConnectAsync();
    }
}
