using Microsoft.AspNetCore.Mvc;
using CSTS.DAL.Models;
using CSTS.DAL.Enum;
using CSTS.DAL.Repository.IRepository;
using CSTS.DAL.AutoMapper.DTOs;
using CSTS.API.ApiServices;


namespace WebApplication1.Controllers
{
    [Route("api/")]
    public class TeamMemberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileService _fileService;
        private readonly ILogger<TeamMemberController> _logger;

        public TeamMemberController(IUnitOfWork unitOfWork, FileService fileService, ILogger<TeamMemberController> logger)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService; 
            _logger = logger;
        }

        [HttpPost("RegisterSupportTeamMember")]
        public async Task<IActionResult> RegisterSupportTeamMember([FromBody] UserDto dto)
        {
            _logger.LogError("LogError");
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");
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
                    return Ok(new APIResponse<bool>(false, string.Join(" , ", ModelState.SelectMany(x => x.Value.Errors).Select(e => e.ErrorMessage))));
                }

                var user = new CSTS.DAL.Models.User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Password = dto.Password,
                    MobileNumber = dto.MobileNumber,
                    Image = _fileService.SaveFile(dto.UserImage , FolderType.Images),
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
