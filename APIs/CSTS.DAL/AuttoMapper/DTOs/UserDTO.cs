using CSTS.DAL.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.AuttoMapper.DTOs
{
    public class CreateUserDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        //public byte[]? Image { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserType UserType { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public List<IFormFile> Attachments { get; set; } // Add this property

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
    public class CreateSupportTeamMemberDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public UserType UserType { get; set; } = UserType.SupportTeamMember;
    }
    public class SupportTeamMemberResponseDTO
    {
        public Guid MemberId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public UserStatus UserStatus { get; set; }
    }
    public class CreateClientDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public UserType UserType { get; set; } = UserType.ExternalClient;
    }
    public class ClientResponseDTO
    {
        public Guid ClientId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public UserStatus UserStatus { get; set; }
    }

    public class ActivationResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class LoginRequest
    {
        [Key]
        public int Id { get; set; }
        public string EmailOrUserName { get; set; }
        public string Password { get; set; }
    }
    public class AttachmentDTO // New DTO for attachments
    {
        public Guid AttachmentId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
