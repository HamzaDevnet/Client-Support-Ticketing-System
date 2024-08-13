using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSTS.DAL.Models;
using CSTS.DAL.Repository.IRepository;
using FluentValidation;
using FluentValidation.Results;
using CSTS.DAL.Enum;

namespace CSTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<User> _validator;

        public UsersController(IUnitOfWork unitOfWork, IValidator<User> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        // GET: api/users
        [HttpGet]
        [CSTS.API.ApiServices.CstsAuth(UserType.ExternalClient)]
        public async Task<ActionResult<APIResponse<IEnumerable<User>>>> Get([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 100)
        {
            try
            {
                var response = _unitOfWork.Users.Get(PageNumber, PageSize);
                return Ok(new APIResponse<IEnumerable<User>>() { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<IEnumerable<User>> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<User>>> Get(Guid id)
        {
            try
            {
                var response = _unitOfWork.Users.GetById(id);
                if (response == null)
                {
                    return NotFound(new APIResponse<User> { Data = null, Code = ResponseCode.Null, Message = "User not found." });
                }
                return Ok(new APIResponse<User> { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<User> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<APIResponse<User>>> Post([FromBody] User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest(new APIResponse<User> { Data = null, Code = ResponseCode.Null, Message = "User cannot be null." });
                }

                // Validate user
                ValidationResult result = _validator.Validate(user);
                if (!result.IsValid)
                {
                    return BadRequest(new APIResponse<User> { Data = null, Code = ResponseCode.Error, Message = result.Errors.ToString() });
                }

                // Support Manager can add Support Team members
                if (user.UserType == UserType.SupportTeamMember || user.UserType == UserType.ExternalClient)
                {
                    var response = _unitOfWork.Users.Add(user);
                    return CreatedAtAction(nameof(Get), new { id = user.UserId }, new APIResponse<User> { Data = user, Code = ResponseCode.Success, Message = "Success" });
                }

                return BadRequest(new APIResponse<User> { Data = null, Code = ResponseCode.Error, Message = "Invalid user type for registration." });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<User> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public async Task<ActionResult<APIResponse<bool>>> Put(Guid id, [FromBody] User user)
        {
            try
            {
                if (user == null || user.UserId != id)
                {
                    return BadRequest(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User is null or ID mismatch." });
                }

                var existingUser = _unitOfWork.Users.GetById(id);
                if (existingUser == null)
                {
                    return NotFound(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });
                }

                // Validate user
                ValidationResult result = _validator.Validate(user);
                if (!result.IsValid)
                {
                    return BadRequest(new APIResponse<bool> { Data = false, Code = ResponseCode.Error, Message = result.Errors.ToString() });
                }

                existingUser.UserName = user.UserName;
                existingUser.FirstName = user.FirstName;
                existingUser.MobileNumber = user.MobileNumber;
                existingUser.Email = user.Email;
                existingUser.Image = user.Image;
                existingUser.DateOfBirth = user.DateOfBirth;
                existingUser.UserType = user.UserType;
                existingUser.Password = user.Password;
                existingUser.Address = user.Address;
                existingUser.RegistrationDate = user.RegistrationDate;
                existingUser.UserStatus = user.UserStatus;

                var response = _unitOfWork.Users.Update(existingUser);
                return Ok(new APIResponse<bool> { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var response = _unitOfWork.Users.Delete(id);
                if (!response)
                {
                    return NotFound(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });
                }

                return Ok(new APIResponse<bool> { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // Activate a user
        [HttpPatch("{id}/activate")]
        public async Task<ActionResult<APIResponse<bool>>> Activate(Guid id)
        {
            try
            {
                var response = _unitOfWork.Users.GetById(id);
                if (response == null)
                {
                    return NotFound(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });
                }

                response.UserStatus = UserStatus.Active;
                var updateResponse = _unitOfWork.Users.Update(response);
                return Ok(new APIResponse<bool> { Data = updateResponse, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // Deactivate a user
        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult<APIResponse<bool>>> Deactivate(Guid id)
        {
            try
            {
                var response = _unitOfWork.Users.GetById(id);
                if (response == null)
                {
                    return NotFound(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });
                }

                response.UserStatus = UserStatus.Deactivated;
                var updateResponse = _unitOfWork.Users.Update(response);
                return Ok(new APIResponse<bool> { Data = updateResponse, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // GET: api/users/support-team-members
        [HttpGet("support-team-members")]
        public async Task<ActionResult<APIResponse<IEnumerable<User>>>> GetSupportTeamMembers()
        {
            try
            {
                var response = _unitOfWork.Users.Find(u => u.UserType == UserType.SupportTeamMember);
                return Ok(new APIResponse<IEnumerable<User>> { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<IEnumerable<User>> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // GET Clients
        [HttpGet("clients")]
        [CSTS.API.ApiServices.CstsAuth(UserType.SupportManager)]
        public async Task<ActionResult<APIResponse<IEnumerable<User>>>> GetExternalClients([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 100)
        {
            try
            {
                var response = _unitOfWork.Users.Find(u => u.UserType == UserType.ExternalClient, PageNumber, PageSize);
                return Ok(new APIResponse<IEnumerable<User>> { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<IEnumerable<User>>(new List<User>() , ex.Message));
            }
        }
    }
}