
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using Microsoft.EntityFrameworkCore;
using CSTS.DAL.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/teammember")]
    public class TeamMemberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamMemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterTeamMember([FromBody] RegisterTeamMemberDTO dto)
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

                var teamMember = new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Password = dto.Password,
                    MobileNumber = dto.MobileNumber,
                    Image = dto.UserImage,
                    DateOfBirth = dto.DateOfBirth,
                    UserName = dto.UserName,
                };


                _context.Users.Add(teamMember);
                await _context.SaveChangesAsync();


                return Ok(new { Message = "Team member registration successful", TeamMember = teamMember, RedirectUrl = "/login" });
            }

            return BadRequest(ModelState);
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
