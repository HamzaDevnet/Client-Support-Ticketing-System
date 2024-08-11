using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using CSTS.DAL.Models;
using Microsoft.AspNetCore.Identity.Data;
using System.Runtime.Intrinsics.X86;
using WebApplication1.Models;
using SlackAPI;
using LoginRequest = WebApplication1.Models.LoginRequest;
using User = CSTS.DAL.Models.User;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public DbSet<RegisterUser> RegisterUser { get; set; }
    public DbSet<LoginRequest> loginRequests { get; set; }
    public DbSet<LoginResponse> LoginResponse { get; set; }
    public DbSet<User1> User1 { get; set; }
    public DbSet<RegisterTeamMember> RegisterTeamMember { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasKey(d => d.UserId);

        modelBuilder.Entity<Ticket>()
        .HasKey(d => d.TicketId);

        modelBuilder.Entity<Comment>()
        .HasKey(d => d.CommentId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedTickets)
            .WithOne(t => t.CreatedBy)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(u => u.AssignedTickets)
            .WithOne(t => t.AssignedTo)
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasMany(t => t.Comments)
            .WithOne(c => c.Ticket)
            .HasForeignKey(c => c.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
