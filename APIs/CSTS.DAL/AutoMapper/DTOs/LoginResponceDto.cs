using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.AutoMapper.DTOs
{
    public class LoginResponseDto
    {
        public string token { set; get; }
        public string fullName { set; get; }
        public string email { set; get; }
        public int userType { set; get; }
    }
}
