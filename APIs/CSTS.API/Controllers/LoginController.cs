using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CSTS.DAL.AutoMapper.DTOs;
using CSTS.DAL.Repository.IRepository;
using CSTS.DAL.Enum;
using CSTS.DAL.Models;
using CSTS.API.ApiServices;



namespace CSTS.API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class LoginController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly FileService _fileService;


        public LoginController(IUnitOfWork unitOfWork, IConfiguration configuration, FileService fileService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _fileService = fileService;
        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] Login loginRequest)
        {
            var user = GetUser_ByUserName(loginRequest.Username);
            if (user == null)
            {
                return Ok(new APIResponse<bool>(false, "Invalid Username"));
            }

            if (user.UserStatus == UserStatus.Deactivated)
            {
                return Ok(new APIResponse<bool>(false, "User is Deactivated."));
            }

            if (user.Password != loginRequest.Password)
            {
                return Ok(new APIResponse<bool>(false, "Incorrect password."));
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
            var secretKey = "your-32-characters-long-secret-key!";
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                    new Claim("UserType", ((int)user.UserType).ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                },
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost("RegisterClient")]
        public async Task<IActionResult> RegisterClient([FromBody] UserDto dto)
        {

            if (ModelState.IsValid)
            {

                var users = _unitOfWork.Users.Find(u => u.UserName == dto.UserName
                                                     || u.Email == dto.Email
                                                     || u.MobileNumber == dto.MobileNumber);

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
                    Image = _fileService.SaveFile(dto.UserImage, FolderType.Images),
                    DateOfBirth = dto.DateOfBirth,
                    UserName = dto.UserName,
                    UserType = UserType.ExternalClient,
                    Address = dto.Address,
                    UserStatus = UserStatus.Active,
                    RegistrationDate = DateTime.Now,

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