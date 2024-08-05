using CSTS.DAL.Enum;
namespace CSTS.DAL.Models {
    public class Ticket
    {
        public Guid TicketId { get; set; } // Primary Key
        public string Product { get; set; }
        public string ProblemDescription { get; set; }
        public string Attachments { get; set; }
        public TicketStatus Status { get; set; } // Enum
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Guid CreatedById { get; set; } // Foreign Key to User
        public User CreatedBy { get; set; }

        public Guid? AssignedToId { get; set; } // Foreign Key to User, Nullable
        public User AssignedTo { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
