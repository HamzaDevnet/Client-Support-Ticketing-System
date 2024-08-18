using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CSTS.DAL.AutoMapper.DTOs;
using CSTS.DAL.Repository.IRepository;
using CSTS.DAL.Enum;
using CSTS.DAL;
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
        private readonly ILogger<LoginController> _logger;


        public LoginController(IUnitOfWork unitOfWork, IConfiguration configuration, FileService fileService, ILogger<LoginController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _fileService = fileService;
        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] Login loginRequest)
        {
            _logger.LogInformation("User {Username} is attempting to login.", loginRequest.Username);
            var user = GetUser_ByUserName(loginRequest.Username);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed for Username: {Username}. Reason: Invalid Username", loginRequest.Username);
                return Ok(new APIResponse<bool>(false, "Invalid Username"));
            }

            if (user.UserStatus == UserStatus.Deactivated)
            {
                _logger.LogWarning("Login attempt failed for Username: {Username}. Reason: User is Deactivated", loginRequest.Username);
                return Ok(new APIResponse<bool>(false, "User is Deactivated."));
            }

            if (!HashingHelper.CompareHash(loginRequest.Password, user.Password))
            {
                _logger.LogWarning("Login attempt failed for Username: {Username}. Reason: Incorrect password", loginRequest.Username);
                return Ok(new APIResponse<bool>(false, "Incorrect password."));
            }

            var token = GenerateJwtToken(user);
            _logger.LogInformation("User {Username} logged in successfully.", loginRequest.Username);

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
            _logger.LogInformation("New client registration attempt for Username: {Username}", dto.UserName);

            if (ModelState.IsValid)
            {

                var users = _unitOfWork.Users.Find(u => u.UserName == dto.UserName
                                                     || u.Email == dto.Email
                                                     || u.MobileNumber == dto.MobileNumber);

                if (users.Any(u => u.Email == dto.Email))
                {
                    _logger.LogWarning("Registration failed for Username: {Username}. Reason: Email already in use.", dto.UserName);
                    ModelState.AddModelError("Email", "Email is already in use.");
                }

                if (users.Any(u => u.MobileNumber == dto.MobileNumber))
                {
                    _logger.LogWarning("Registration failed for Username: {Username}. Reason: Mobile number already in use.", dto.UserName);
                    ModelState.AddModelError("MobileNumber", "Mobile number is already in use.");
                }

                if (users.Any(u => u.UserName == dto.UserName))
                {
                    _logger.LogWarning("Registration failed for Username: {Username}. Reason: Username already in use.", dto.UserName);
                    ModelState.AddModelError("UserName", "UserName is already in use.");
                }

                if (dto.Password.Count() < 8)
                {
                    return Ok(new APIResponse<bool>(false, "Password at least 8 characters"));
                }

                if (!ModelState.IsValid)
                {
                    return Ok(new APIResponse<bool>(false, string.Join(" , ", ModelState.SelectMany(x => x.Value.Errors).Select(e => e.ErrorMessage))));
                }

                var user = new CSTS.DAL.Models.User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Password = HashingHelper.GetHashString(dto.Password),
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
                _logger.LogInformation("User {Username} registered successfully.", dto.UserName);
                return Ok(new APIResponse<bool>(true));
            }
            _logger.LogWarning("Registration attempt failed due to invalid model state for Username: {Username}.", dto.UserName);
            return Ok(new APIResponse<bool>(false, string.Concat(" , ", ModelState.SelectMany(x => x.Value.Errors).SelectMany(e => e.ErrorMessage))));
        }

        private CSTS.DAL.Models.User? GetUser_ByUserName(string UserName)
        {
            return _unitOfWork.Users.Find(u => u.UserName.ToLower() == UserName.ToLower()).FirstOrDefault();
        }

    }
}