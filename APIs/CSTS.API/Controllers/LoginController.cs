using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO;

using CSTS.DAL.AutoMapper.DTOs;
using CSTS.DAL.Repository.IRepository;
using CSTS.DAL.Enum;
using CSTS.DAL.Migrations;
using CSTS.DAL.Models;
using System.Linq;




namespace CSTS.API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class LoginController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public LoginController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] Login loginRequest)
        {
            var user = GetUser_ByUserName(loginRequest.Username);
            if (user == null || user.Password != loginRequest.Password)
            {
                return Ok(new APIResponse<bool>(false));
            }

            var token = GenerateJwtToken(user);

            LoginResponseDto response = new LoginResponseDto()
            {
                token = token,
                email = user.Email,
                fullName = user.FullName,
                userType = (int)user.UserType
            };

            return Ok(new APIResponse<LoginResponseDto>(response));

        }


        private string GenerateJwtToken(CSTS.DAL.Models.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Upn, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, ((int)user.UserType).ToString())  // Using UserType enum
                }),

                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenstr = token.ToString();

            return tokenHandler.WriteToken(token);
        }


        [HttpPost("RegisterClient")]
        public async Task<IActionResult> RegisterClient([FromBody] RegisterDto dto)
        {

            if (ModelState.IsValid)
            {

                var users = _unitOfWork.Users.Find(u => string.Equals(u.UserName, dto.UserName, StringComparison.CurrentCultureIgnoreCase)
                                                     || string.Equals(u.Email, dto.Email, StringComparison.CurrentCultureIgnoreCase)
                                                     || string.Equals(u.MobileNumber, dto.MobileNumber, StringComparison.CurrentCultureIgnoreCase)
                            );

                if (users.Any(u => u.Email == dto.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                }

                if (users.Any(u => u.MobileNumber == dto.MobileNumber))
                {
                    ModelState.AddModelError("MobileNumber", "Mobile number is already in use.");
                }

                if (users.Any(u => u.UserName == dto.UserName))
                {
                    ModelState.AddModelError("UserName", "UserName is already in use.");
                }

                if (!ModelState.IsValid)
                {
                    return Ok(new APIResponse<bool>(false, string.Concat(" , ", ModelState.SelectMany(x => x.Value.Errors).SelectMany(e => e.ErrorMessage))));
                }

                var user = new CSTS.DAL.Models.User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Password = dto.Password,
                    MobileNumber = dto.MobileNumber,
                    Image = dto.UserImage,
                    DateOfBirth = dto.DateOfBirth,
                    UserName = dto.UserName,
                    UserType = UserType.ExternalClient,
                    //Address = dto.Address
                };

                _unitOfWork.Users.Add(user);

                return Ok(new APIResponse<bool>(true));
            }

            return Ok(new APIResponse<bool>(false, string.Concat(" , ", ModelState.SelectMany(x => x.Value.Errors).SelectMany(e => e.ErrorMessage))));
        }


        [HttpPost("RegisterSupportTeamMember")]
        public async Task<IActionResult> RegisterSupportTeamMember([FromBody] RegisterDto dto)
        {

            if (ModelState.IsValid)
            {

                var users = _unitOfWork.Users.Find(u => string.Equals(u.UserName, dto.UserName, StringComparison.CurrentCultureIgnoreCase)
                                                     || string.Equals(u.Email, dto.Email, StringComparison.CurrentCultureIgnoreCase)
                                                     || string.Equals(u.MobileNumber, dto.MobileNumber, StringComparison.CurrentCultureIgnoreCase)
                            );

                if (users.Any(u => u.Email == dto.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                }

                if (users.Any(u => u.MobileNumber == dto.MobileNumber))
                {
                    ModelState.AddModelError("MobileNumber", "Mobile number is already in use.");
                }

                if (users.Any(u => u.UserName == dto.UserName))
                {
                    ModelState.AddModelError("UserName", "UserName is already in use.");
                }

                if (!ModelState.IsValid)
                {
                    return Ok(new APIResponse<bool>(false, string.Concat(" , ", ModelState.SelectMany(x => x.Value.Errors).SelectMany(e => e.ErrorMessage))));
                }

                var user = new CSTS.DAL.Models.User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Password = dto.Password,
                    MobileNumber = dto.MobileNumber,
                    Image = dto.UserImage,
                    DateOfBirth = dto.DateOfBirth,
                    UserName = dto.UserName,
                    UserType = UserType.SupportTeamMember,
                    //Address = dto.Address
                };

                _unitOfWork.Users.Add(user);

                return Ok(new APIResponse<bool>(true));
            }

            return Ok(new APIResponse<bool>(false, string.Concat(" , ", ModelState.SelectMany(x => x.Value.Errors).SelectMany(e => e.ErrorMessage))));
        }


        private CSTS.DAL.Models.User? GetUser_ByUserName(string UserName)
        {
            return _unitOfWork.Users.Find(u => u.UserName.ToLower() == UserName.ToLower()).FirstOrDefault();
        }

    }
}