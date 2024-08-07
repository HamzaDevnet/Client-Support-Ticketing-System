using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSTS.DAL.Models;
using CSTS.DAL.Repository.IRepository;
using CSTS.DAL.Enum;
using FluentValidation;
using FluentValidation.Results;
using CSTS.DAL.DTOs;

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
        public async Task<ActionResult<WebResponse<IEnumerable<UserSummaryDTO>>>> Get()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                var userDtos = users.Data.Select(user => new UserSummaryDTO
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserType = user.UserType
                }).ToList();
                return Ok(new WebResponse<IEnumerable<UserSummaryDTO>> { Data = userDtos, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<IEnumerable<UserSummaryDTO>> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WebResponse<UserResponseDTO>>> Get(Guid id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user.Data == null)
                    return NotFound(new WebResponse<UserResponseDTO> { Data = null, Code = ResponseCode.Null, Message = "User not found." });

                var userDto = new UserResponseDTO
                {
                    UserId = user.Data.UserId,
                    UserName = user.Data.UserName,
                    FullName = user.Data.FullName,
                    MobileNumber = user.Data.MobileNumber,
                    Email = user.Data.Email,
                    UserStatus = user.Data.UserStatus
                };
                return Ok(new WebResponse<UserResponseDTO> { Data = userDto, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<UserResponseDTO> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<WebResponse<UserResponseDTO>>> Post([FromBody] CreateUserDTO createUserDto)
        {
            try
            {
                var user = new User
                {
                    UserName = createUserDto.UserName,
                    FullName = createUserDto.FullName,
                    MobileNumber = createUserDto.MobileNumber,
                    Email = createUserDto.Email,
                    DateOfBirth = createUserDto.DateOfBirth,
                    UserType = createUserDto.UserType,
                    Password = createUserDto.Password,
                    Address = createUserDto.Address,
                    RegistrationDate = DateTime.UtcNow
                };

                ValidationResult result = _validator.Validate(user);
                if (!result.IsValid)
                    return BadRequest(new WebResponse<UserResponseDTO> { Data = null, Code = ResponseCode.Error, Message = string.Join(", ", result.Errors.Select(e => e.ErrorMessage)) });

                var response = await _unitOfWork.Users.AddAsync(user);
                if (!response.Data)
                    return StatusCode(500, new WebResponse<UserResponseDTO> { Data = null, Code = ResponseCode.Error, Message = "Failed to add user" });

                var responseDto = new UserResponseDTO
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    MobileNumber = user.MobileNumber,
                    Email = user.Email,
                    UserStatus = user.UserStatus
                };

                return CreatedAtAction(nameof(Get), new { id = user.UserId }, new WebResponse<UserResponseDTO> { Data = responseDto, Code = ResponseCode.Success, Message = "User created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<UserResponseDTO> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // PUT api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<WebResponse<UserResponseDTO>>> Put(Guid id, [FromBody] UpdateUserDTO updateUserDto)
        {
            try
            {
                var existingUser = await _unitOfWork.Users.GetByIdAsync(id);
                if (existingUser.Data == null)
                    return NotFound(new WebResponse<UserResponseDTO> { Data = null, Code = ResponseCode.Null, Message = "User not found." });

                existingUser.Data.UserName = updateUserDto.UserName;
                existingUser.Data.FullName = updateUserDto.FullName;
                existingUser.Data.MobileNumber = updateUserDto.MobileNumber;
                existingUser.Data.Email = updateUserDto.Email;
                existingUser.Data.Image = updateUserDto.Image;
                existingUser.Data.DateOfBirth = updateUserDto.DateOfBirth;
                existingUser.Data.UserStatus = updateUserDto.UserStatus;
                existingUser.Data.Password = updateUserDto.Password;
                existingUser.Data.Address = updateUserDto.Address;

                ValidationResult validationResult = _validator.Validate(existingUser.Data);
                if (!validationResult.IsValid)
                    return BadRequest(new WebResponse<UserResponseDTO> { Data = null, Code = ResponseCode.Error, Message = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)) });

                var response = await _unitOfWork.Users.UpdateAsync(existingUser.Data);
                if (!response.Data)
                    return StatusCode(500, new WebResponse<UserResponseDTO> { Data = null, Code = ResponseCode.Error, Message = "Failed to update user" });

                var updatedDto = new UserResponseDTO
                {
                    UserId = existingUser.Data.UserId,
                    UserName = existingUser.Data.UserName,
                    FullName = existingUser.Data.FullName,
                    MobileNumber = existingUser.Data.MobileNumber,
                    Email = existingUser.Data.Email,
                    UserStatus = existingUser.Data.UserStatus
                };
                return Ok(new WebResponse<UserResponseDTO> { Data = updatedDto, Code = ResponseCode.Success, Message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<UserResponseDTO> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // DELETE api/users/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<WebResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var response = await _unitOfWork.Users.DeleteAsync(id);
                if (!response.Data)
                    return NotFound(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });

                return Ok(new WebResponse<bool> { Data = true, Code = ResponseCode.Success, Message = "User deleted successfully." });
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
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user.Data == null)
                    return NotFound(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });

                if (user.Data.UserStatus == UserStatus.Active)
                    return BadRequest(new WebResponse<bool> { Data = false, Code = ResponseCode.Warning, Message = "User is already active." });

                user.Data.UserStatus = UserStatus.Active;
                var updateResponse = await _unitOfWork.Users.UpdateAsync(user.Data);
                return Ok(new WebResponse<bool> { Data = updateResponse.Data, Code = ResponseCode.Success, Message = "User activated successfully." });
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
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user.Data == null)
                    return NotFound(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });

                if (user.Data.UserStatus == UserStatus.Deactivated)
                    return BadRequest(new WebResponse<bool> { Data = false, Code = ResponseCode.Warning, Message = "User is already deactivated." });

                user.Data.UserStatus = UserStatus.Deactivated;
                var updateResponse = await _unitOfWork.Users.UpdateAsync(user.Data);
                return Ok(new WebResponse<bool> { Data = updateResponse.Data, Code = ResponseCode.Success, Message = "User deactivated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }
    }
}
