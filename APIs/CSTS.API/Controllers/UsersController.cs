using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSTS.DAL.Models;
using CSTS.DAL.Repository.IRepository;
using FluentValidation;
using FluentValidation.Results;
using CSTS.DAL.Enum;
using CSTS.API.ApiServices;
using CSTS.DAL.AutoMapper.DTOs;
using AutoMapper;

namespace CSTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<User> _validator;
        private readonly IMapper _mapper;

        public UsersController(IUnitOfWork unitOfWork, IValidator<User> validator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
        }

        // GET: api/users
        [HttpGet]
        //[CstsAuth(UserType.ExternalClient)]
        public async Task<ActionResult<APIResponse<IEnumerable<UserResponseDTO>>>> Get([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 100)
        {
            try
            {
                var response = _unitOfWork.Users.Get(PageNumber, PageSize).Select(u => _mapper.Map<UserResponseDTO>(u));
                return Ok(new APIResponse<IEnumerable<UserResponseDTO>>() { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<IEnumerable<UserResponseDTO>> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<UserResponseDTO>>> Get(Guid id)
        {
            try
            {
                var response = _mapper.Map<UserResponseDTO>(_unitOfWork.Users.GetById(id));

                if (response == null)
                {
                    return Ok(new APIResponse<UserResponseDTO> { Data = null, Code = ResponseCode.Null, Message = "User not found." });
                }
                return Ok(new APIResponse<UserResponseDTO> { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<UserResponseDTO> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public async Task<ActionResult<APIResponse<bool>>> Put([FromRoute] Guid id, [FromBody] UserDto inputUser)
        {
            try
            {
                User user = _mapper.Map<User>(inputUser);

                if (user == null)
                {
                    return Ok(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User is null or ID mismatch." });
                }

                var existingUser = _unitOfWork.Users.GetById(id);
                if (existingUser == null)
                {
                    return Ok(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });
                }

                // Validate user
                ValidationResult result = _validator.Validate(user);
                if (!result.IsValid)
                {
                    return Ok(new APIResponse<bool> { Data = false, Code = ResponseCode.Error, Message = result.Errors.ToString() });
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
                    return Ok(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });
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
        //[CstsAuth(UserType.SupportManager)]
        public async Task<ActionResult<APIResponse<bool>>> Activate(Guid id)
        {
            try
            {
                var response = _unitOfWork.Users.GetById(id);
                if (response == null)
                {
                    return Ok(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });
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
        //[CstsAuth(UserType.SupportManager)]
        public async Task<ActionResult<APIResponse<bool>>> Deactivate(Guid id)
        {
            try
            {
                var response = _unitOfWork.Users.GetById(id);
                if (response == null)
                {
                    return Ok(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "User not found." });
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
        public async Task<ActionResult<APIResponse<IEnumerable<UserResponseDTO>>>> GetSupportTeamMembers()
        {
            try
            {
                var response = _unitOfWork.Users.Find(u => u.UserType == UserType.SupportTeamMember).Select(u => _mapper.Map<UserResponseDTO>(u));
                return Ok(new APIResponse<IEnumerable<UserResponseDTO>> { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<IEnumerable<UserResponseDTO>> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // GET Clients
        [HttpGet("clients")]
        //[CstsAuth(UserType.SupportManager)]
        public async Task<ActionResult<APIResponse<IEnumerable<UserResponseDTO>>>> GetExternalClients([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 100)
        {
            try
            {
                var response = _unitOfWork.Users.Find(u => u.UserType == UserType.ExternalClient, PageNumber, PageSize).Select(u => _mapper.Map<UserResponseDTO>(u));
                return Ok(new APIResponse<IEnumerable<UserResponseDTO>> { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<IEnumerable<UserResponseDTO>>(new List<UserResponseDTO>(), ex.Message));
            }
        }
    }
}