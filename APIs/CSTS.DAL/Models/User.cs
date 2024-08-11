using CSTS.DAL.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSTS.DAL.Models
{
    public class User
    {
        public Guid UserId { get; set; } // Primary Key
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        //IFormFile
        public byte[]? Image { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserStatus UserStatus { get; set; } // Enum
        public UserType UserType { get; set; } // Enum
        public string Password { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string EmailOrUserName { get; set; }

        public ICollection<Ticket>? CreatedTickets { get; set; } // Tickets created by this user
        public ICollection<Ticket>? AssignedTickets { get; set; } // Tickets assigned to this user

        public ICollection<Comment>? Comments { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

    }
}