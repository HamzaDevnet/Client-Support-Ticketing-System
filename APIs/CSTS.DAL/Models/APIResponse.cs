using CSTS.DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.Models
{
    public class WebResponse<T>
    {
        public ResponseCode Code { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }

}
