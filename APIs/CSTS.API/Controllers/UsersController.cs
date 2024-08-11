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
        public async Task<ActionResult<WebResponse<IEnumerable<User>>>> Get()
        {
            try
            {
                var response = await _unitOfWork.Users.GetAllAsync();
                return Ok(new WebResponse<IEnumerable<User>>() { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<IEnumerable<User>> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WebResponse<User>>> Get(Guid id)
        {
            try
            {
                var response = await _unitOfWork.Users.GetByIdAsync(id);
                if (response.Data == null)
                {
                    return NotFound(new WebResponse<User> { Data = null, Code = ResponseCode.Null, Message = "User not found." });
                }
                return Ok(new WebResponse<User> { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<User> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<WebResponse<User>>> Post([FromBody] User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest(new WebResponse<User> { Data = null, Code = ResponseCode.Null, Message = "User cannot be null." });
                }

                // Validate user
                ValidationResult result = _validator.Validate(user);
                if (!result.IsValid)
                {
                    return BadRequest(new WebResponse<User> { Data = null, Code = ResponseCode.Error, Message = result.Errors.ToString() });
                }

                // Support Manager can add Support Team members
                if (user.UserType == UserType.SupportTeamMember || user.UserType == UserType.ExternalClient)
                {
                    var response = await _unitOfWork.Users.AddAsync(user);
                    return CreatedAtAction(nameof(Get), new { id = user.UserId }, new WebResponse<User> { Data = user, Code = ResponseCode.Success, Message = "Success" });
                }

                return BadRequest(new WebResponse<User> { Data = null, Code = ResponseCode.Error, Message = "Invalid user type for registration." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<User> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public async Task<ActionResult<WebResponse<bool>>> Put(Guid id, [FromBody] User user)
        {
            try
            {
                if (user == null || user.UserId != id)
                {
                    return BadRequest(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User is null or ID mismatch." });
                }

                var existingUser = await _unitOfWork.Users.GetByIdAsync(id);
                if (existingUser.Data == null)
                {
                    return NotFound(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });
                }

                // Validate user
                ValidationResult result = _validator.Validate(user);
                if (!result.IsValid)
                {
                    return BadRequest(new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = result.Errors.ToString() });
                }

                existingUser.Data.UserName = user.UserName;
                existingUser.Data.FirstName = user.FirstName;
                existingUser.Data.MobileNumber = user.MobileNumber;
                existingUser.Data.Email = user.Email;
                existingUser.Data.Image = user.Image;
                existingUser.Data.DateOfBirth = user.DateOfBirth;
                existingUser.Data.UserType = user.UserType;
                existingUser.Data.Password = user.Password;
                existingUser.Data.Address = user.Address;
                existingUser.Data.RegistrationDate = user.RegistrationDate;
                existingUser.Data.UserStatus = user.UserStatus;

                var response = await _unitOfWork.Users.UpdateAsync(existingUser.Data);
                return Ok(new WebResponse<bool> { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WebResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var response = await _unitOfWork.Users.DeleteAsync(id);
                if (!response.Data)
                {
                    return NotFound(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });
                }

                return Ok(new WebResponse<bool> { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // Activate a user
        [HttpPatch("{id}/activate")]
        public async Task<ActionResult<WebResponse<bool>>> Activate(Guid id)
        {
            try
            {
                var response = await _unitOfWork.Users.GetByIdAsync(id);
                if (response.Data == null)
                {
                    return NotFound(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });
                }

                response.Data.UserStatus = UserStatus.Active;
                var updateResponse = await _unitOfWork.Users.UpdateAsync(response.Data);
                return Ok(new WebResponse<bool> { Data = updateResponse.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // Deactivate a user
        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult<WebResponse<bool>>> Deactivate(Guid id)
        {
            try
            {
                var response = await _unitOfWork.Users.GetByIdAsync(id);
                if (response.Data == null)
                {
                    return NotFound(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });
                }

                response.Data.UserStatus = UserStatus.Deactivated;
                var updateResponse = await _unitOfWork.Users.UpdateAsync(response.Data);
                return Ok(new WebResponse<bool> { Data = updateResponse.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // GET: api/users/support-team-members
        [HttpGet("support-team-members")]
        public async Task<ActionResult<WebResponse<IEnumerable<User>>>> GetSupportTeamMembers()
        {
            try
            {
                var response = await _unitOfWork.Users.FindAsync(u => u.UserType == UserType.SupportTeamMember);
                return Ok(new WebResponse<IEnumerable<User>> { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<IEnumerable<User>> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // GET: api/users/external-clients
        [HttpGet("external-clients")]
        public async Task<ActionResult<WebResponse<IEnumerable<User>>>> GetExternalClients()
        {
            try
            {
                var response = await _unitOfWork.Users.FindAsync(u => u.UserType == UserType.ExternalClient);
                return Ok(new WebResponse<IEnumerable<User>> { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<IEnumerable<User>> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }
    }
}