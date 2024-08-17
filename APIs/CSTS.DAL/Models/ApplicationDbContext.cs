using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using CSTS.DAL.Models;
using Microsoft.AspNetCore.Identity.Data;
using System.Runtime.Intrinsics.X86;

using User = CSTS.DAL.Models.User;
using CSTS.DAL.Enum;

public class ApplicationDbContext : DbContext
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Comment> Comments { get; set; }



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

        // Seed data
        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId =Guid.Parse( "725c2c65-2fc0-45ec-b28f-fc56f268c225"),
                UserName = "Manger",
                FirstName = "Admin",
                LastName = "Admin",
                MobileNumber = "1234567890",
                Email = "Manger@example.com",
                UserStatus = UserStatus.Active,
                UserType = UserType.SupportManager,
                Password = "123456", // Consider using a hashed password in production
                Address = "Address1",
                RegistrationDate = DateTime.Parse("2024 - 08 - 14 15:30:00"),
                DateOfBirth = new DateTime(1990, 1, 1)
            }
           
        );
    }
}