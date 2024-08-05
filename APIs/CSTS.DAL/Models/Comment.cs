namespace CSTS.DAL.Models
{
    public class Comment
    {
        public Guid CommentId { get; set; } // Primary Key
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid TicketId { get; set; } // Foreign Key to Ticket
        public Ticket Ticket { get; set; }

        public Guid UserId { get; set; } // Foreign Key to User
        public User User { get; set; }
    }
}