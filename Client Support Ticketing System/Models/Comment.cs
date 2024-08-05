
public class Comment
{
    public Guid CommentId { get; set; } // Primary Key
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }

    public int TicketId { get; set; } // Foreign Key to Ticket
    public Ticket Ticket { get; set; }

    public int UserId { get; set; } // Foreign Key to User
    public User User { get; set; }
}
