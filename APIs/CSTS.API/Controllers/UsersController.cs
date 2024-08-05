using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSTS.DAL.Models;
using CSTS.DAL;
using CSTS.DAL.Repository.IRepository;
using FluentValidation;
using FluentValidation.Results;

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
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return Ok(users);
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null.");
            }

            // Validate user
            ValidationResult result = _validator.Validate(user);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] User user)
        {
            if (user == null || user.UserId != id)
            {
                return BadRequest("User is null or ID mismatch.");
            }

            var existingUser = await _unitOfWork.Users.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Validate user
            ValidationResult result = _validator.Validate(user);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            existingUser.UserName = user.UserName;
            existingUser.UserName = user.FullName;
            existingUser.MobileNumber = user.MobileNumber;
            existingUser.Email = user.Email;
            existingUser.Image = user.Image;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.UserType = user.UserType;
            existingUser.Password = user.Password;
            existingUser.Address = user.Address;
            existingUser.RegistrationDate = user.RegistrationDate;

            await _unitOfWork.Users.UpdateAsync(existingUser);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
