using CSTS.DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.DTOs
{
    public class CreateUserDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public byte[]? Image { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserType UserType { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
    }

    public class UserResponseDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public UserStatus UserStatus { get; set; }
    }

    public class UpdateUserDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public byte[]? Image { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserStatus UserStatus { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
    }

    public class UserSummaryDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public UserType UserType { get; set; }
    }
    public class ActivationResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
