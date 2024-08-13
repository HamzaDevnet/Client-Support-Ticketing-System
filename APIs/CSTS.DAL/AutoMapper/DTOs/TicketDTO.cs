// TicketDTO.cs
using CSTS.DAL.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.AutoMapper.DTOs
{
    public class CreateTicketDTO
    {
        public string Product { get; set; }
        public string ProblemDescription { get; set; }
        public List<IFormFile> Attachments { get; set; }
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
        public List<AttachmentDto> Attachments { get; set; }
    }

    public class UpdateTicketDTO
    {
        public TicketStatus Status { get; set; }
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

    
    public class AssignTicketDTO
    {
        public Guid TicketId { get; set; }
        public Guid? AssignedTo { get; set; }
    }

}
