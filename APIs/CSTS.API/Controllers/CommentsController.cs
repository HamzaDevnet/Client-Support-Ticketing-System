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
        public async Task<ActionResult<APIResponse<IEnumerable<Comment>>>> Get()
        {
            try
            {
                var response = _unitOfWork.Comments.Get();
                return Ok(new APIResponse<IEnumerable<Comment>>() { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<IEnumerable<Comment>> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // GET api/comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<Comment>>> Get(Guid id)
        {
            try
            {
                var response = _unitOfWork.Comments.GetById(id);
                if (response == null)
                {
                    return NotFound(new APIResponse<Comment> { Data = null, Code = ResponseCode.Null, Message = "Comment not found." });
                }
                return Ok(new APIResponse<Comment> { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<Comment> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // POST api/comments
        [HttpPost]
        public async Task<ActionResult<APIResponse<Comment>>> Post([FromBody] Comment comment)
        {
            try
            {
                if (comment == null)
                {
                    return BadRequest(new APIResponse<Comment> { Data = null, Code = ResponseCode.Null, Message = "Comment cannot be null." });
                }

                // Validate comment
                ValidationResult result = _validator.Validate(comment);
                if (!result.IsValid)
                {
                    return BadRequest(new APIResponse<Comment> { Data = null, Code = ResponseCode.Error, Message = result.Errors.ToString() });
                }

                var response = _unitOfWork.Comments.Add(comment);
                return CreatedAtAction(nameof(Get), new { id = comment.CommentId }, new APIResponse<Comment> { Data = comment, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<Comment> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // PUT api/comments/5
        [HttpPut("{id}")]
        public async Task<ActionResult<APIResponse<bool>>> Put(Guid id, [FromBody] Comment comment)
        {
            try
            {
                if (comment == null || comment.CommentId != id)
                {
                    return BadRequest(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "Comment is null or ID mismatch." });
                }

                var existingComment = _unitOfWork.Comments.GetById(id);
                if (existingComment == null)
                {
                    return NotFound(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "Comment not found." });
                }

                // Validate comment
                ValidationResult result = _validator.Validate(comment);
                if (!result.IsValid)
                {
                    return BadRequest(new APIResponse<bool> { Data = false, Code = ResponseCode.Error, Message = result.Errors.ToString() });
                }

                existingComment.Content = comment.Content;
                existingComment.CreatedDate = comment.CreatedDate;
                existingComment.UserId = comment.UserId;
                existingComment.TicketId = comment.TicketId;

                var response = _unitOfWork.Comments.Update(existingComment);
                return Ok(new APIResponse<bool> { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // DELETE api/comments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var response = _unitOfWork.Comments.Delete(id);
                if (!response)
                {
                    return NotFound(new APIResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "Comment not found." });
                }

                return Ok(new APIResponse<bool> { Data = response, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }
    }
}