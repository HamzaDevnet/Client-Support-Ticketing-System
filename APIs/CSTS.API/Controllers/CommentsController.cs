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
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(IUnitOfWork unitOfWork, IValidator<CreateCommentDTO> validator, IMapper mapper, ILogger<CommentsController> logger)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [CstsAuth(UserType.SupportTeamMember, UserType.ExternalClient)]
        public async Task<ActionResult<APIResponse<IEnumerable<CommentResponseDTO>>>> GetComments([FromQuery] Guid ticketId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            _logger.LogInformation("Fetching comments for ticket {TicketId} - Page: {PageNumber}, PageSize: {PageSize}", ticketId, pageNumber, pageSize);

            try
            {
                var comments = _unitOfWork.Comments.Find(c => c.TicketId == ticketId, pageNumber, pageSize, c => c.User, c => c.Ticket);

                var commentDtos = _mapper.Map<IEnumerable<CommentResponseDTO>>(comments);
                _logger.LogInformation("Fetched {CommentCount} comments successfully for ticket {TicketId}.", commentDtos.Count(), ticketId);


                return Ok(new APIResponse<IEnumerable<CommentResponseDTO>>(commentDtos, ResponseCode.Success, "Success"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching comments for ticket {TicketId}.", ticketId);
                return Ok(new APIResponse<IEnumerable<CommentResponseDTO>>(null, ResponseCode.Error, ex.Message));
            }
        }

        /* ******************  WHY DO WE NEED TWO GET METHODS FOR COMMENTS BASED ON USER ROLE????
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
                return Ok(new APIResponse<IEnumerable<CommentResponseDTO>>(null, ResponseCode.Error, ex.Message));
            }
        }

        // GET: api/comments/support
        [HttpGet("support")]
        [CstsAuth(UserType.SupportTeamMember)]
        public async Task<ActionResult<APIResponse<IEnumerable<CommentResponseDTO>>>> GetSupportComments([FromQuery] Guid ticketId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            try
            {
                var comments = _unitOfWork.Comments.Find(c => c.TicketId == ticketId && (c.User.UserType == UserType.SupportTeamMember || c.User.UserType == UserType.ExternalClient), pageNumber, pageSize, c => c.User, c => c.Ticket);
                var commentDtos = _mapper.Map<IEnumerable<CommentResponseDTO>>(comments);

                return Ok(new APIResponse<IEnumerable<CommentResponseDTO>>(commentDtos, ResponseCode.Success, "Success"));
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<IEnumerable<CommentResponseDTO>>(null, ResponseCode.Error, ex.Message));
            }
        } */


        // GET api/comments/{id}
        [HttpGet("{id}")]
        [CstsAuth(UserType.SupportTeamMember, UserType.ExternalClient)]
        public async Task<ActionResult<APIResponse<CommentResponseDTO>>> Get(Guid id)
        {
            _logger.LogInformation("Fetching comment with ID {CommentId}", id);
            try
            {
                var comment = _unitOfWork.Comments.GetById(id);
                if (comment == null)
                {
                    _logger.LogWarning("Comment with ID {CommentId} not found.", id);
                    return Ok(new APIResponse<CommentResponseDTO>(null, ResponseCode.Null, "Comment not found."));
                }

                var commentDto = _mapper.Map<CommentResponseDTO>(comment);
                _logger.LogInformation("Fetched comment with ID {CommentId} successfully.", id);

                return Ok(new APIResponse<CommentResponseDTO>(commentDto, ResponseCode.Success, "Success"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching comment with ID {CommentId}.", id);
                return Ok(new APIResponse<CommentResponseDTO>(null, ResponseCode.Error, ex.Message));
            }
        }

        // POST api/comments
        [HttpPost]
        [CstsAuth(UserType.SupportTeamMember, UserType.ExternalClient)]
        public async Task<ActionResult<APIResponse<CommentResponseDTO>>> Post([FromBody] CreateCommentDTO createCommentDto)
        {
            _logger.LogInformation("Posting new comment");
            try
            {
                if (createCommentDto == null)
                {
                    _logger.LogWarning("CreateCommentDTO is null.");
                    return Ok(new APIResponse<CommentResponseDTO>(null, ResponseCode.Null, "Comment cannot be null."));
                }

                ValidationResult result = _validator.Validate(createCommentDto);
                if (!result.IsValid)
                {
                    _logger.LogWarning("Validation failed for CreateCommentDTO. Errors: {Errors}", string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
                    return Ok(new APIResponse<CommentResponseDTO>(null, ResponseCode.Error, string.Join(", ", result.Errors.Select(e => e.ErrorMessage))));
                }

                var ticket = _unitOfWork.Tickets.GetById(createCommentDto.TicketId);
                if (ticket == null)
                {
                    _logger.LogWarning("Ticket with ID {TicketId} not found.", createCommentDto.TicketId);
                    return Ok(new APIResponse<CommentResponseDTO>(null, ResponseCode.Null, "Ticket not found."));
                }

                var comment = _mapper.Map<Comment>(createCommentDto);
                comment.CommentId = Guid.NewGuid();
                comment.CreatedDate = DateTime.UtcNow;

                var response = _unitOfWork.Comments.Add(comment);
                if (!response)
                {
                    _logger.LogError("Failed to add comment.");
                    return Ok(new APIResponse<CommentResponseDTO>(null, ResponseCode.Error, "Failed to add comment."));
                }

                var commentResponseDto = _mapper.Map<CommentResponseDTO>(comment);
                _logger.LogInformation("Comment with ID {CommentId} added successfully.", comment.CommentId);
                return CreatedAtAction(nameof(Get), new { id = comment.CommentId }, new APIResponse<CommentResponseDTO>(commentResponseDto, ResponseCode.Success, "Success"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment.");
                return Ok(new APIResponse<CommentResponseDTO>(null, ResponseCode.Error, ex.Message));
            }
        }
    }
}
