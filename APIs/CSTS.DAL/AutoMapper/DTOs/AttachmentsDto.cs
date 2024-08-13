using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.AutoMapper.DTOs
{
    public class AttachmentDto
    {
        public Guid AttachmentId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
