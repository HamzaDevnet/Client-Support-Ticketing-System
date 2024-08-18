using CSTS.DAL.AutoMapper.DTOs;
using CSTS.DAL.Enum;
using CSTS.DAL.Models;
using CSTS.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/")]
    public class ForgetPasswordController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ForgetPasswordController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        // [HttpPost("ForgotPassword")]
        // public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        // {
        //     var user = _unitOfWork.Users.Get(u => u.Email == request.Email);
        //     if (user == null)
        //     {
        //         return BadRequest(new { Success = false, Code = ResponseCode.Error, Message = "Email not found." });
        //     }
        //     var resetToken = GeneratePasswordResetToken(user);
        //     bool emailSent = await SendPasswordResetEmail(user.Email, resetToken);
        //     if (emailSent)
        //     {
        //         return Ok(new { Success = true, Code = ResponseCode.Success, Message = "The email was sent successfully." });
        //     }
        //     else
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Code = ResponseCode.Null, Message = "Failed to send password reset email." });
        //     }
        // }

        // [HttpPost("ResetPassword")]
        // public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        // {
        //     var userId = GetUserIdFromToken();
        //     var user = _unitOfWork.Users.GetById(userId.Value);
        //     if (user == null)
        //     {
        //         return BadRequest(new { Success = false, Code = ResponseCode.Error, Message = "Invalid or expired token." });
        //     }
        // 
        //     user.Password = request.NewPassword; 
        //     _unitOfWork.Users.Update(user);
        //     await _unitOfWork.CompleteAsync();
        // 
        //     return Ok(new { Success = true, Code = ResponseCode.Success, Message = "Password reset successful." });
        // }

        // [HttpPost("ChangePassword")]
        // public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        // {
        //     var userId = GetUserIdFromToken();
        //     if (userId == null)
        //     {
        //         return Unauthorized(new { Success = false, Code = ResponseCode.Unauthorized, Message = "Unauthorized." });
        //     }
        //     var user = await _unitOfWork.Users.GetByIdAsync(userId.Value);
        //     if (user == null)
        //     {
        //         return NotFound(new { Success = false, Code = ResponseCode.Error, Message = "User not found." });
        //     }
        //     if (!ValidatePassword(user, request.CurrentPassword))
        //     {
        //         return BadRequest(new { Success = false, Code = ResponseCode.Error, Message = "Current password is incorrect." });
        //     }
        //     var changeResult = await ChangePassword(user, request.NewPassword);
        //     if (changeResult)
        //     {
        //         return Ok(new { Success = true, Code = ResponseCode.Success, Message = "Password changed successfully." });
        //     }
        //     else
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Code = ResponseCode.Null, Message = "Failed to change password." });
        //     }
        // }

        private string GenerateJwtToken(User user)
        {
            var secretKey = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                    new Claim("UserType", ((int)user.UserType).ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                },
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GeneratePasswordResetToken(User user)
        {
            return Guid.NewGuid().ToString(); // Placeholder
        }

        private async Task<bool> SendPasswordResetEmail(string email, string token)
        {
            try
            {
                // Implement email sending logic here
                return true; 
            }
            catch (Exception ex)
            {
                return false; 
            }
        }

        private async Task<User> ValidatePasswordResetToken(string token)
        {
            // Implement token validation logic here
            return null; 
        }

        private bool ValidatePassword(User user, string password)
        {
            // Implement your password validation logic
            return user.Password == password; 
        }

        private async Task<bool> ChangePassword(User user, string newPassword)
        {
            // Implement your password change logic
            user.Password = newPassword; 
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private Guid? GetUserIdFromToken()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : (Guid?)null;
        }
        
    }
}
