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
using CSTS.DAL.AutoMapper.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using CSTS.API.ApiServices;

namespace CSTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCommentDTO> _validator;
        private readonly IMapper _mapper;

        public CommentsController(IUnitOfWork unitOfWork, IValidator<CreateCommentDTO> validator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
        }

        // GET: api/comments/client
        [HttpGet("client")]
        [CstsAuth(UserType.ExternalClient)]
        public async Task<ActionResult<APIResponse<IEnumerable<CommentResponseDTO>>>> GetClientComments([FromQuery] Guid ticketId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            try
            {
                var comments = _unitOfWork.Comments.Find(c => c.TicketId == ticketId && c.User.UserType == UserType.ExternalClient, pageNumber, pageSize, c => c.User, c => c.Ticket);
                var commentDtos = _mapper.Map<IEnumerable<CommentResponseDTO>>(comments);

                return Ok(new APIResponse<IEnumerable<CommentResponseDTO>>(commentDtos, ResponseCode.Success, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<IEnumerable<CommentResponseDTO>>(null, ResponseCode.Error, ex.Message));
            }
        }

        // GET: api/comments/support
        [HttpGet("support")]
        [CstsAuth(UserType.SupportTeamMember)]
        public async Task<ActionResult<APIResponse<IEnumerable<CommentResponseDTO>>>> GetSupportComments([FromQuery] Guid ticketId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            try
            {
                var comments = _unitOfWork.Comments.Find(c => c.TicketId == ticketId && (c.User.UserType == UserType.SupportTeamMember || c.User.UserType == UserType.SupportManager), pageNumber, pageSize, c => c.User, c => c.Ticket);
                var commentDtos = _mapper.Map<IEnumerable<CommentResponseDTO>>(comments);

                return Ok(new APIResponse<IEnumerable<CommentResponseDTO>>(commentDtos, ResponseCode.Success, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<IEnumerable<CommentResponseDTO>>(null, ResponseCode.Error, ex.Message));
            }
        }

        // GET api/comments/{id}
        [HttpGet("{id}")]
        //[Authorize(Roles = "ExternalClient,SupportTeamMember")]
        public async Task<ActionResult<APIResponse<CommentResponseDTO>>> Get(Guid id)
        {
            try
            {
                var comment = _unitOfWork.Comments.GetById(id);
                if (comment == null)
                {
                    return NotFound(new APIResponse<CommentResponseDTO>(null, ResponseCode.Null, "Comment not found."));
                }

                var commentDto = _mapper.Map<CommentResponseDTO>(comment);
                return Ok(new APIResponse<CommentResponseDTO>(commentDto, ResponseCode.Success, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<CommentResponseDTO>(null, ResponseCode.Error, ex.Message));
            }
        }

        // POST api/comments
        [HttpPost]
        //[Authorize(Roles = "ExternalClient,SupportTeamMember")]
        public async Task<ActionResult<APIResponse<CommentResponseDTO>>> Post([FromBody] CreateCommentDTO createCommentDto)
        {
            try
            {
                if (createCommentDto == null)
                {
                    return BadRequest(new APIResponse<CommentResponseDTO>(null, ResponseCode.Null, "Comment cannot be null."));
                }

                // Validate comment DTO
                ValidationResult result = _validator.Validate(createCommentDto);
                if (!result.IsValid)
                {
                    return BadRequest(new APIResponse<CommentResponseDTO>(null, ResponseCode.Error, string.Join(", ", result.Errors.Select(e => e.ErrorMessage))));
                }

                var ticket = _unitOfWork.Tickets.GetById(createCommentDto.TicketId);
                if (ticket == null)
                {
                    return NotFound(new APIResponse<CommentResponseDTO>(null, ResponseCode.Null, "Ticket not found."));
                }

                var comment = _mapper.Map<Comment>(createCommentDto);
                comment.CommentId = Guid.NewGuid();
                comment.CreatedDate = DateTime.UtcNow;

                var response = _unitOfWork.Comments.Add(comment);
                if (!response)
                {
                    return StatusCode(500, new APIResponse<CommentResponseDTO>(null, ResponseCode.Error, "Failed to add comment."));
                }

                var commentResponseDto = _mapper.Map<CommentResponseDTO>(comment);

                return CreatedAtAction(nameof(Get), new { id = comment.CommentId }, new APIResponse<CommentResponseDTO>(commentResponseDto, ResponseCode.Success, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<CommentResponseDTO>(null, ResponseCode.Error, ex.Message));
            }
        }
    }
}
