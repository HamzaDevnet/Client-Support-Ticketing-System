public class User
{
    public Guid UserId { get; set; } // Primary Key
    public string FullName { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }
    public string UserImage { get; set; }
    public DateTime DateOfBirth { get; set; }
    public UserType UserType { get; set; } // Enum
    public string Password { get; set; }
    public string Address { get; set; }
  //  public UserStatus Status { get; set; } // Enum
    public DateTime RegistrationDate { get; set; }

    public ICollection<Ticket> CreatedTickets { get; set; } // Tickets created by this user
    public ICollection<Ticket> AssignedTickets { get; set; } // Tickets assigned to this user
}

public enum UserType
{
    ExternalClient,
    SupportTeamMember,
    SupportManager
}

//public enum UserStatus
//{
//    Active,
//    Deactivated
//}
