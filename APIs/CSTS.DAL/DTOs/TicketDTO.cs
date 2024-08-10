using CSTS.DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.DTOs
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
    }
}
