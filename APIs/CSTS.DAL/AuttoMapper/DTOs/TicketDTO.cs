// TicketDTO.cs
using CSTS.DAL.Enum;
using System;

namespace CSTS.DAL.AuttoMapper.DTOs
{
    public class CreateTicketDTO
    {
        public string Product { get; set; }
        public string ProblemDescription { get; set; }
        public string Attachments { get; set; }
        public Guid? AssignedToId { get; set; }
    }

    public class TicketResponseDTO
    {
        public Guid TicketId { get; set; }
        public string Product { get; set; }
        public string ProblemDescription { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AssignedToUserName { get; set; }
        public string AssignedToFullName { get; set; }
        public List<CommentResponseDTO> Comments { get; set; } // New property for comments
        public List<AttachmentDTO> Attachments { get; set; }
    }

    public class UpdateTicketDTO
    {
        public string Product { get; set; }
        public string ProblemDescription { get; set; }
        public string Attachments { get; set; }
        public TicketStatus Status { get; set; }
        public Guid? AssignedToId { get; set; }
    }

    public class UpdateResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class TicketSummaryDTO
    {
        public Guid TicketId { get; set; }
        public string Product { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AssignedToFullName { get; set; }
    }

    public class AttachmentDTO // New DTO for attachments
    {
        public Guid AttachmentId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
    public class AssignTicketDTO
    {
        public Guid TicketId { get; set; }
        public Guid? AssignedTo { get; set; }
    }
}
