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
        public async Task<ActionResult<IEnumerable<UserSummaryDTO>>> Get()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var userDtos = users.Data.Select(user => new UserSummaryDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserType = user.UserType
            }).ToList();
            return Ok(userDtos);
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> Get(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user.Data == null)
                return NotFound("User not found.");

            var userDto = new UserResponseDTO
            {
                UserId = user.Data.UserId,
                UserName = user.Data.UserName,
                FullName = user.Data.FullName,
                MobileNumber = user.Data.MobileNumber,
                Email = user.Data.Email,
                UserStatus = user.Data.UserStatus
            };
            return Ok(userDto);
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> Post([FromBody] CreateUserDTO createUserDto)
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
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var response = await _unitOfWork.Users.AddAsync(user);
            if (!response.Data)
                return StatusCode(500, "Failed to add user");

            var responseDto = new UserResponseDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FullName = user.FullName,
                MobileNumber = user.MobileNumber,
                Email = user.Email,
                UserStatus = user.UserStatus
            };

            return CreatedAtAction(nameof(Get), new { id = user.UserId }, responseDto);
        }

        // PUT api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDTO>> Put(Guid id, [FromBody] UpdateUserDTO updateUserDto)
        {
            var existingUser = await _unitOfWork.Users.GetByIdAsync(id);
            if (existingUser.Data == null)
                return NotFound("User not found.");

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
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var response = await _unitOfWork.Users.UpdateAsync(existingUser.Data);
            if (!response.Data)
                return StatusCode(500, "Failed to update user");

            var updatedDto = new UserResponseDTO
            {
                UserId = existingUser.Data.UserId,
                UserName = existingUser.Data.UserName,
                FullName = existingUser.Data.FullName,
                MobileNumber = existingUser.Data.MobileNumber,
                Email = existingUser.Data.Email,
                UserStatus = existingUser.Data.UserStatus
            };
            return Ok(updatedDto);
        }

        // DELETE api/users/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var response = await _unitOfWork.Users.DeleteAsync(id);
            if (!response.Data)
                return NotFound("User not found.");

            return Ok(true);
        }

        // Activate a user
        [HttpPatch("{id}/activate")]
        public async Task<ActionResult<ActivationResponseDTO>> Activate(Guid id)
        {
            var userResponse = await _unitOfWork.Users.GetByIdAsync(id);
            if (userResponse.Data == null)
                return NotFound(new ActivationResponseDTO { Success = false, Message = "User not found." });

            userResponse.Data.UserStatus = UserStatus.Active;
            var updateResponse = await _unitOfWork.Users.UpdateAsync(userResponse.Data);

            return Ok(new ActivationResponseDTO
            {
                Success = updateResponse.Data,
                Message = updateResponse.Data ? "User activated successfully." : "Failed to activate user."
            });
        }

        // Deactivate a user
        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult<ActivationResponseDTO>> Deactivate(Guid id)
        {
            var userResponse = await _unitOfWork.Users.GetByIdAsync(id);
            if (userResponse.Data == null)
                return NotFound(new ActivationResponseDTO { Success = false, Message = "User not found." });

            userResponse.Data.UserStatus = UserStatus.Deactivated;
            var updateResponse = await _unitOfWork.Users.UpdateAsync(userResponse.Data);

            return Ok(new ActivationResponseDTO
            {
                Success = updateResponse.Data,
                Message = updateResponse.Data ? "User deactivated successfully." : "Failed to deactivate user."
            });
        }
    }
}
