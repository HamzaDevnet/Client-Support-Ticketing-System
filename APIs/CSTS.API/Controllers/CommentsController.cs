using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSTS.DAL.Models;
using CSTS.DAL.Repository.IRepository;
using FluentValidation;
using FluentValidation.Results;
using CSTS.DAL.Enum;
using CSTS.DAL.DTOs;
using static CSTS.DAL.DTOs.CommentResponseDTO;

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
        public async Task<ActionResult<IEnumerable<CommentSummaryDTO>>> Get()
        {
            var response = await _unitOfWork.Comments.GetAllAsync();
            var dtos = response.Data.Select(c => new CommentSummaryDTO
            {
                CommentId = c.CommentId,
                Content = c.Content
            }).ToList();
            return Ok(dtos);
        }

        // GET api/comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentResponseDTO>> Get(Guid id)
        {
            var response = await _unitOfWork.Comments.GetByIdAsync(id);
            if (response.Data == null)
            {
                return NotFound("Comment not found.");
            }

            var dto = new CommentResponseDTO
            {
                CommentId = response.Data.CommentId,
                Content = response.Data.Content,
                CreatedDate = response.Data.CreatedDate,
                UserName = response.Data.User?.UserName // Assuming User navigation property
            };
            return Ok(dto);
        }

        // POST api/comments
        [HttpPost]
        public async Task<ActionResult<CommentResponseDTO>> Post([FromBody] CreateCommentDTO dto)
        {
            var comment = new Comment
            {
                Content = dto.Content,
                UserId = dto.UserId,
                TicketId = dto.TicketId,
                CreatedDate = DateTime.UtcNow
            };

            var result = _validator.Validate(comment);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));
            }

            var response = await _unitOfWork.Comments.AddAsync(comment);
            if (!response.Data)
            {
                return StatusCode(500, "Failed to add comment");
            }

            var responseDto = new CommentResponseDTO
            {
                CommentId = comment.CommentId,
                Content = comment.Content,
                CreatedDate = comment.CreatedDate,
                UserName = comment.User?.UserName // Assuming User navigation property
            };
            return CreatedAtAction(nameof(Get), new { id = comment.CommentId }, responseDto);
        }

        // PUT api/comments/5
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(Guid id, [FromBody] UpdateCommentDTO dto)
        {
            var existingComment = await _unitOfWork.Comments.GetByIdAsync(id);
            if (existingComment.Data == null)
            {
                return NotFound("Comment not found.");
            }

            existingComment.Data.Content = dto.Content;

            var result = _validator.Validate(existingComment.Data);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));
            }

            var response = await _unitOfWork.Comments.UpdateAsync(existingComment.Data);
            return Ok(response.Data);
        }

        // DELETE api/comments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var response = await _unitOfWork.Comments.DeleteAsync(id);
            if (!response.Data)
            {
                return NotFound("Comment not found.");
            }
            return Ok(true);
        }
    }
}
