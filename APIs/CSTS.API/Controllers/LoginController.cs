using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO;

using SlackAPI;





namespace CSTS.API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<DAL.Models.User> _userRepository;
        private readonly IConfiguration _configuration;

        public LoginController(IRepository<DAL.Models.User> userRepository, IConfiguration configuration, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] DAL.Models.User loginRequest)
        {
            var response = await _userRepository.GetUserByEmailOrUserName(loginRequest.EmailOrUserName);
            if (response == null || response.Data == null || response.Data.Password != loginRequest.Password)
            {
                return Unauthorized(new LoginResponse { });
            }
            var user = response.Data;
            var token = GenerateJwtToken(user);

            return Ok(new { Success = true, Message = "Login successful", Token = token, RedirectUrl = "/main" });
            //return RedirectToAction("Index", "Home"); 

        }


        private string GenerateJwtToken(CSTS.DAL.Models.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                     new Claim(ClaimTypes.Role, user.UserType.ToString())  // Using UserType enum
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
                if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                }

                if (await _context.Users.AnyAsync(u => u.MobileNumber == dto.MobileNumber))
                {
                    ModelState.AddModelError("MobileNumber", "Mobile number is already in use.");
                }

                if (await _context.Users.AnyAsync(u => u.UserName == dto.UserName))
                {
                    ModelState.AddModelError("UserName", "UserName is already in use.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
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
                    // UserType = dto.UserType,
                    //Address = dto.Address
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();


                return Ok(new { Message = "Registration successful", User = user, RedirectUrl = "/login" });
                //return RedirectToAction("Index", "Home");
            }

            return BadRequest(ModelState);
        }

    }
}