using CSTS.API.Controllers;
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
        private readonly ILogger<ForgetPasswordController> _logger;
        public ForgetPasswordController(IUnitOfWork unitOfWork, IConfiguration configuration , ILogger<ForgetPasswordController> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            _logger.LogInformation("Received password reset request for Email: {Email}", request.Email);
            var user = _unitOfWork.Users.Get().FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                _logger.LogWarning("Password reset request failed: Email not found: {Email}", request.Email);
                return BadRequest(new { Success = false, Code = ResponseCode.Error, Message = "Email not found." });
            }

            var resetToken = GeneratePasswordResetToken(user);
            bool emailSent = await SendPasswordResetEmail(user.Email, resetToken);

            if (emailSent)
            {
                _logger.LogInformation("Password reset email sent successfully to: {Email}", user.Email);
                return Ok(new { Success = true, Code = ResponseCode.Success, Message = "The email was sent successfully." });
                /*{
                           "success": true,
                           "code": 200,
                           "message": "The email was sent successfully."
                  }  */

                // return Ok(new APIResponse<bool>(true, "The email was sent successfully."));
                /*    {
                          "code": 400,
                          "data": true,
                          "message": "The email was sent successfully."
                      } */
            }
            else
            {
                _logger.LogError("Failed to send password reset email to: {Email}", user.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Code = ResponseCode.Null, Message = "Failed to send password reset email." });
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            _logger.LogInformation("Attempting to reset password with provided token.");
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                _logger.LogWarning("Invalid or expired token provided for password reset.");
                return BadRequest(new { Success = false, Code = ResponseCode.Error, Message = "Invalid or expired token." });
            }
            var user =  _unitOfWork.Users.GetById(userId.Value);
            if (user == null)
            {
                _logger.LogWarning("User not found for the provided token.");
                return BadRequest(new { Success = false, Code = ResponseCode.Error, Message = "Invalid or expired token." });
            }

            user.Password = request.NewPassword; 
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Password reset successfully for User ID: {UserId}", user.UserId);
            return Ok(new { Success = true, Code = ResponseCode.Success, Message = "Password reset successful." });
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            _logger.LogInformation("Received change password request.");
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                _logger.LogWarning("Unauthorized change password attempt.");
                return Unauthorized(new { Success = false, Code = ResponseCode.Unauthorized, Message = "Unauthorized." });
            }

            var user = _unitOfWork.Users.GetById(userId.Value);
            if (user == null)
            {
                _logger.LogWarning("User not found for password change.");
                return NotFound(new { Success = false, Code = ResponseCode.Error, Message = "User not found." });
            }

            if (!ValidatePassword(user, request.CurrentPassword))
            {
                _logger.LogWarning("Password change failed: Current password is incorrect for User ID: {UserId}", user.UserId);
                return BadRequest(new { Success = false, Code = ResponseCode.Error, Message = "Current password is incorrect." });
            }

            var changeResult = await ChangePassword(user, request.NewPassword);
            if (changeResult)
            {
                _logger.LogInformation("Password changed successfully for User ID: {UserId}", user.UserId);
                return Ok(new { Success = true, Code = ResponseCode.Success, Message = "Password changed successfully." });
            }
            else
            {
                _logger.LogError("Failed to change password for User ID: {UserId}", user.UserId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Code = ResponseCode.Null, Message = "Failed to change password." });
            }
        }

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
            _logger.LogInformation("Generating password reset token for User ID: {UserId}", user.UserId);
            return Guid.NewGuid().ToString(); // Placeholder
        }

        private async Task<bool> SendPasswordResetEmail(string email, string token)
        {
            try
            {
                _logger.LogInformation("Sending password reset email to: {Email}", email);
                // Implement email sending logic here
                return true; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending password reset email to: {Email}", email);
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
            _logger.LogInformation("Changing password for User ID: {UserId}", user.UserId);
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
                _logger.LogWarning("Authorization token not found.");
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : (Guid?)null;
        }
        
    }
}
