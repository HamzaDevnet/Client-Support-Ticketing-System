using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CSTS.DAL.Models;
using CSTS.DAL.Enum;
using System;
using System.Linq;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            
            if (context.Users.Any())
            {
                return;  
            }

           
            context.Users.AddRange(
                new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = "Hamza",
                    FirstName = "Hamzeh",
                    LastName = "User",
                    MobileNumber = "1234567890",
                    Email = "Hamza@example.com",
                    UserStatus = UserStatus.Active,
                    UserType = UserType.SupportTeamMember,
                    Password = "123456",
                    Address = "Address1",
                    RegistrationDate = DateTime.Now,
                    DateOfBirth = new DateTime(1990, 1, 1)
                },
                new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = "client",
                    FirstName = "Client",
                    LastName = "User",
                    MobileNumber = "0987654321",
                    Email = "client@example.com",
                    UserStatus = UserStatus.Active,
                    UserType = UserType.ExternalClient,
                    Password = "ClientPassword123!",
                    Address = "Client Address",
                    RegistrationDate = DateTime.Now,
                    DateOfBirth = new DateTime(1995, 5, 5)
                },
                new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = "support",
                    FirstName = "Support",
                    LastName = "Team",
                    MobileNumber = "1122334455",
                    Email = "support@example.com",
                    UserStatus = UserStatus.Active,
                    UserType = UserType.SupportTeamMember,
                    Password = "SupportPassword123!",
                    Address = "Support Address",
                    RegistrationDate = DateTime.Now,
                    DateOfBirth = new DateTime(1992, 3, 3)
                }
            );

            context.SaveChanges(); 
        }
    }
}
