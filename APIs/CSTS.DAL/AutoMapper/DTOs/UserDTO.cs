using CSTS.DAL.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.AutoMapper.DTOs
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        //IFormFile
        public byte[]? UserImage { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }

    }

    public class UserResponseDTO
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public UserStatus UserStatus { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

}
