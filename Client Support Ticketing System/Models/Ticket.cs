

public class Ticket
{
    public Guid TicketId { get; set; } // Primary Key
    public string Product { get; set; }
    public string ProblemDescription { get; set; }
    public string Attachments { get; set; }
    public TicketStatus Status { get; set; } // Enum
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public int CreatedById { get; set; } // Foreign Key to User
    public User CreatedBy { get; set; }

    public int? AssignedToId { get; set; } // Foreign Key to User, Nullable
    public User AssignedTo { get; set; }

    public ICollection<Comment> Comments { get; set; }
}

public enum TicketStatus
{
    New,
    Assigned,
    InProgress,
    Closed
}
