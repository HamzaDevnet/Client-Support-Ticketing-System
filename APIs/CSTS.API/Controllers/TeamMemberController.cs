
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using Microsoft.EntityFrameworkCore;
using CSTS.DAL.Models;
using CSTS.DAL.Enum;
using CSTS.DAL.Repository.IRepository;

namespace WebApplication1.Controllers
{
    public class TeamMemberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        public TeamMemberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        [HttpPost("RegisterSupportTeamMember")]
        public async Task<IActionResult> RegisterSupportTeamMember([FromBody] RegisterDto dto)
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
                    Image = dto.UserImage,
                    DateOfBirth = dto.DateOfBirth,
                    UserName = dto.UserName,
                    UserType = UserType.SupportTeamMember,
                    Address = dto.Address,

                };

                _unitOfWork.Users.Add(user);

                return Ok(new APIResponse<bool>(true));
            }

            return Ok(new APIResponse<bool>(false, string.Concat(" , ", ModelState.SelectMany(x => x.Value.Errors).SelectMany(e => e.ErrorMessage))));
        }
    }
}
