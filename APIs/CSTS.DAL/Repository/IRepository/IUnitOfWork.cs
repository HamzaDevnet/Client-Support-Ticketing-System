using CSTS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Ticket> Tickets { get; }
        IRepository<Comment> Comments { get; }
        Task<int> CompleteAsync();
        Task<bool> CanConnectAsync();
    }
}
