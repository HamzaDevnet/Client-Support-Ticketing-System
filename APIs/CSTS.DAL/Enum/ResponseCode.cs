using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.Enum
{
    public enum ResponseCode
    {
        Success = 200,
        Warning = 300,
        Error = 400,
        Unauthorized = 401,
        Null = 500
    }
}
