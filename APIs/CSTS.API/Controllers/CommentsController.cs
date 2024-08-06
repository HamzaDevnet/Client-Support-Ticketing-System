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
        public async Task<ActionResult<WebResponse<IEnumerable<Comment>>>> Get()
        {
            try
            {
                var response = await _unitOfWork.Comments.GetAllAsync();
                return Ok(new WebResponse<IEnumerable<Comment>>() { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<IEnumerable<Comment>> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // GET api/comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WebResponse<Comment>>> Get(Guid id)
        {
            try
            {
                var response = await _unitOfWork.Comments.GetByIdAsync(id);
                if (response.Data == null)
                {
                    return NotFound(new WebResponse<Comment> { Data = null, Code = ResponseCode.Null, Message = "Comment not found." });
                }
                return Ok(new WebResponse<Comment> { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<Comment> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // POST api/comments
        [HttpPost]
        public async Task<ActionResult<WebResponse<Comment>>> Post([FromBody] Comment comment)
        {
            try
            {
                if (comment == null)
                {
                    return BadRequest(new WebResponse<Comment> { Data = null, Code = ResponseCode.Null, Message = "Comment cannot be null." });
                }

                // Validate comment
                ValidationResult result = _validator.Validate(comment);
                if (!result.IsValid)
                {
                    return BadRequest(new WebResponse<Comment> { Data = null, Code = ResponseCode.Error, Message = result.Errors.ToString() });
                }

                var response = await _unitOfWork.Comments.AddAsync(comment);
                return CreatedAtAction(nameof(Get), new { id = comment.CommentId }, new WebResponse<Comment> { Data = comment, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<Comment> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // PUT api/comments/5
        [HttpPut("{id}")]
        public async Task<ActionResult<WebResponse<bool>>> Put(Guid id, [FromBody] Comment comment)
        {
            try
            {
                if (comment == null || comment.CommentId != id)
                {
                    return BadRequest(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "Comment is null or ID mismatch." });
                }

                var existingComment = await _unitOfWork.Comments.GetByIdAsync(id);
                if (existingComment.Data == null)
                {
                    return NotFound(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "Comment not found." });
                }

                // Validate comment
                ValidationResult result = _validator.Validate(comment);
                if (!result.IsValid)
                {
                    return BadRequest(new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = result.Errors.ToString() });
                }

                existingComment.Data.Content = comment.Content;
                existingComment.Data.CreatedDate = comment.CreatedDate;
                existingComment.Data.UserId = comment.UserId;
                existingComment.Data.TicketId = comment.TicketId;

                var response = await _unitOfWork.Comments.UpdateAsync(existingComment.Data);
                return Ok(new WebResponse<bool> { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // DELETE api/comments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WebResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var response = await _unitOfWork.Comments.DeleteAsync(id);
                if (!response.Data)
                {
                    return NotFound(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "Comment not found." });
                }

                return Ok(new WebResponse<bool> { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }
    }
}
