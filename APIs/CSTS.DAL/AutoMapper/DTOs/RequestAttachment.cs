using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.AutoMapper.DTOs
{
    public class RequestAttachment
    {
        public byte[] File { get; set; }

        private string? fileExtension { get; set; }
        private string? fileName { get; set; }

        public string? FileExtension
        {
            set
            {
                fileExtension = value;
            }
        }
        public string? FileName
        {
            set

            {
                fileName = value;
            }
        }

        public string Extension
        {
            get
            {
                return (fileExtension != null ? fileExtension : Path.GetExtension(fileName));
            }
        }

    }
}
