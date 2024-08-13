using System;

namespace CSTS.DAL.AutoMapper.DTOs
{
    public class CreateCommentDTO
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid TicketId { get; set; }
    }

    public class CommentResponseDTO
    {
        public Guid CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public Guid TicketId { get; set; }
    }
}
