using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.Models
{
    public class Attachment
    {
        public Guid AttachmentId { get; set; } // Primary Key
        public string FileName { get; set; }
        public string FileUrl { get; set; }

        public Guid TicketId { get; set; } // Foreign Key to Ticket
        public Ticket Ticket { get; set; }
    }
}
