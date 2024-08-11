using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Repository;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO;
using CSTS.DAL.Models;
using Microsoft.AspNetCore.Identity.Data;
using NuGet.Protocol.Plugins;
using SlackAPI;


namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/")]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public LoginController(IUserRepository userRepository, IConfiguration configuration, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] Models.LoginRequest loginRequest)
        {
            var user = _userRepository.GetUserByEmailOrUserName(loginRequest.EmailOrUserName);  
            if (user == null || user.Password != loginRequest.Password)
            {
                return Unauthorized(new LoginResponse { });
            }

            var token = GenerateJwtToken(user);

            return Ok(new { Success = true, Message = "Login successful", Token = token, RedirectUrl = "/main" });
            //return RedirectToAction("Index", "Home"); 

        }


        private string GenerateJwtToken(User1 user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {

            if (ModelState.IsValid)
            {
                if (await _context.RegisterUser.AnyAsync(u => u.Email == dto.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                }

                if (await _context.RegisterUser.AnyAsync(u => u.MobileNumber == dto.MobileNumber))
                {
                    ModelState.AddModelError("MobileNumber", "Mobile number is already in use.");
                }

                if (await _context.RegisterUser.AnyAsync(u => u.UserName == dto.UserName))
                {
                    ModelState.AddModelError("UserName", "UserName is already in use.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new RegisterUser
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Password = dto.Password,
                    MobileNumber = dto.MobileNumber,
                    UserImage = dto.UserImage,
                    DateOfBirth = dto.DateOfBirth,
                    UserName = dto.UserName,
                    // UserType = dto.UserType,
                    //Address = dto.Address
                };

                _context.RegisterUser.Add(user);
                await _context.SaveChangesAsync();


                return Ok(new { Message = "Registration successful", User = user, RedirectUrl = "/login" });
                //return RedirectToAction("Index", "Home");
            }

            return BadRequest(ModelState);
        }

    }
}