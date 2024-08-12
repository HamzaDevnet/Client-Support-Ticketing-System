using CSTS.DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.Models
{
    public class APIResponse<T>
    {
        public APIResponse() { }

        public APIResponse( T response) 
        { 
            Code = ResponseCode.Success;
            Message = "";
            Data = response;
        }
        public APIResponse(T response,string errorMessage)
        {
            Code = ResponseCode.Error;
            Message = errorMessage;
            Data = response;
        }
        public APIResponse(T response, ResponseCode responseCode , string errorMessage)
        {
            Code = responseCode;
            Message = errorMessage;
            Data = response;
        }


        public ResponseCode Code { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }

}
