using Microsoft.AspNetCore.Mvc;
using CSTS.DAL.Models;
using CSTS.DAL;
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
            _logger.LogInformation("Starting RegisterSupportTeamMember process"); 
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Checking if user already exists with the same email, mobile number, or username");
                var users = _unitOfWork.Users.Find(u => u.UserName == dto.UserName
                                                     || u.Email == dto.Email
                                                     || u.MobileNumber == dto.MobileNumber);

                if (users.Any(u => u.Email == dto.Email))
                {
                    _logger.LogWarning("Email {Email} is already in use.", dto.Email);
                    ModelState.AddModelError("Email", "Email is already in use.");
                }

                if (users.Any(u => u.MobileNumber == dto.MobileNumber))
                {
                    _logger.LogWarning("Mobile number {MobileNumber} is already in use.", dto.MobileNumber);
                    ModelState.AddModelError("MobileNumber", "Mobile number is already in use.");
                }

                if (users.Any(u => u.UserName == dto.UserName))
                {
                    _logger.LogWarning("Username {UserName} is already in use.", dto.UserName);
                    ModelState.AddModelError("UserName", "UserName is already in use.");
                }

                if (dto.Password.Count() < 8)
                {
                    _logger.LogWarning("Password is less than 8 characters.");
                    return Ok(new APIResponse<bool>(false, "Password at least 8 characters"));
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model state is invalid after validation checks. Errors: {Errors}", string.Join(" , ", ModelState.SelectMany(x => x.Value.Errors).Select(e => e.ErrorMessage)));
                    return Ok(new APIResponse<bool>(false, string.Join(" , ", ModelState.SelectMany(x => x.Value.Errors).Select(e => e.ErrorMessage))));
                }

                _logger.LogInformation("Creating new user");

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
                    UserType = UserType.SupportTeamMember,
                    Address = dto.Address,

                };

                _unitOfWork.Users.Add(user);
                _logger.LogInformation("User {UserName} created successfully", dto.UserName);
                return Ok(new APIResponse<bool>(true));
            }
            _logger.LogError( "An error occurred while registering support team member.");
            return Ok(new APIResponse<bool>(false, string.Concat(" , ", ModelState.SelectMany(x => x.Value.Errors).SelectMany(e => e.ErrorMessage))));
        }
    }
}
