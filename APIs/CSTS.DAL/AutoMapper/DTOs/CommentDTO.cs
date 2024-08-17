using CSTS.DAL.Enum;
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
        public Guid UserId { get; set; }
        public string UserImage { get; set; }
        public Guid CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FullName { get; set; }
        public UserType userType { get; set; }
        public Guid TicketId { get; set; }
    }
}
