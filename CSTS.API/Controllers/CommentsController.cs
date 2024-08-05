using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    public class CommentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Comment> _validator;

        public CommentsController(IUnitOfWork unitOfWork, IValidator<Comment> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        // GET: api/comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> Get()
        {
            var comments = await _unitOfWork.Comments.GetAllAsync();
            return Ok(comments);
        }

        // GET api/comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> Get(Guid id)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        // POST api/comments
        [HttpPost]
        public async Task<ActionResult<Comment>> Post([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest("Comment cannot be null.");
            }

            // Validate comment
            ValidationResult result = _validator.Validate(comment);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(Get), new { id = comment.CommentId }, comment);
        }

        // PUT api/comments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Comment comment)
        {
            if (comment == null || comment.CommentId != id)
            {
                return BadRequest("Comment is null or ID mismatch.");
            }

            var existingComment = await _unitOfWork.Comments.GetByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            // Validate comment
            ValidationResult result = _validator.Validate(comment);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            existingComment.Content = comment.Content;
            existingComment.CreatedDate = comment.CreatedDate;
            existingComment.UserId = comment.UserId;
            existingComment.TicketId = comment.TicketId;

            await _unitOfWork.Comments.UpdateAsync(existingComment);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE api/comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            await _unitOfWork.Comments.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
