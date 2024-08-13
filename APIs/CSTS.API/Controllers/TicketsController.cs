using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSTS.DAL.Models;
using CSTS.DAL.Repository.IRepository;
using FluentValidation;
using CSTS.DAL.Enum;
using CSTS.DAL.AutoMapper.DTOs;
using Microsoft.EntityFrameworkCore;
using CSTS.API.ApiServices;
using System.Security.Claims;

namespace CSTS.API.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Ticket> _validator;
        private readonly FileService _fileService;

        public TicketsController(IUnitOfWork unitOfWork, IValidator<Ticket> validator, FileService fileService)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _fileService = fileService;
        }

        // GET: api/tickets (Client)
        [HttpGet("tickets")]
        [CstsAuth(UserType.ExternalClient)]
        public async Task<ActionResult<APIResponse<IEnumerable<TicketSummaryDTO>>>> GetClientTickets([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 100)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get user id from token
                var tickets = _unitOfWork.Tickets.Get(PageNumber, PageSize, t => t.AssignedTo).Where(t => t.CreatedById == Guid.Parse(userId));
                var ticketDtos = tickets.Select(t => new TicketSummaryDTO
                {
                    TicketId = t.TicketId,
                    Product = t.Product,
                    Status = t.Status,
                    CreatedDate = t.CreatedDate,
                    AssignedToFullName = t.AssignedTo != null ? t.AssignedTo.FullName : "Not Assigned"
                }).ToList();

                return Ok(new APIResponse<IEnumerable<TicketSummaryDTO>>(ticketDtos) { Message = "Tickets retrieved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<IEnumerable<TicketSummaryDTO>>(null, $"Internal server error: {ex.Message}"));
            }
        }

        // GET api/tickets/support-tickets
        [HttpGet("support-tickets")]
        [CstsAuth(UserType.SupportTeamMember)]
        public async Task<ActionResult<APIResponse<IEnumerable<TicketSummaryDTO>>>> GetSupportTickets([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 100)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get user id from token
                var tickets = _unitOfWork.Tickets.Get(PageNumber, PageSize, t => t.AssignedTo).Where(t => t.AssignedToId == Guid.Parse(userId) && t.Status == TicketStatus.Assigned);
                var ticketDtos = tickets.Select(t => new TicketSummaryDTO
                {
                    TicketId = t.TicketId,
                    Product = t.Product,
                    Status = t.Status,
                    CreatedDate = t.CreatedDate,
                    AssignedToFullName = t.AssignedTo != null ? t.AssignedTo.FullName : "Not Assigned"
                }).ToList();

                return Ok(new APIResponse<IEnumerable<TicketSummaryDTO>>(ticketDtos) { Message = "Assigned tickets retrieved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<IEnumerable<TicketSummaryDTO>>(null, $"Internal server error: {ex.Message}"));
            }
        }

        // GET api/tickets/manager-tickets
        [HttpGet("manager-tickets")]
        [CstsAuth(UserType.SupportManager)]
        public async Task<ActionResult<APIResponse<IEnumerable<TicketSummaryDTO>>>> GetManagerTickets([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 100)
        {
            try
            {
                var tickets = _unitOfWork.Tickets.Get(PageNumber, PageSize, t => t.AssignedTo);
                var ticketDtos = tickets.Select(t => new TicketSummaryDTO
                {
                    TicketId = t.TicketId,
                    Product = t.Product,
                    Status = t.Status,
                    CreatedDate = t.CreatedDate,
                    AssignedToFullName = t.AssignedTo != null ? t.AssignedTo.FullName : "Not Assigned"
                }).ToList();

                return Ok(new APIResponse<IEnumerable<TicketSummaryDTO>>(ticketDtos) { Message = "All tickets retrieved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<IEnumerable<TicketSummaryDTO>>(null, $"Internal server error: {ex.Message}"));
            }
        }

        // GET api/tickets/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<TicketResponseDTO>>> Get(Guid id)
        {
            try
            {
                var ticket = _unitOfWork.Tickets.GetQueryable()
                    .Include(t => t.AssignedTo)
                    .Include(t => t.Comments).ThenInclude(c => c.User)
                    .Include(t => t.Attachments)
                    .FirstOrDefault(t => t.TicketId == id);

                if (ticket == null)
                    return NotFound(new APIResponse<TicketResponseDTO>(null, "Ticket not found."));

                var ticketDto = new TicketResponseDTO
                {
                    TicketId = ticket.TicketId,
                    Product = ticket.Product,
                    ProblemDescription = ticket.ProblemDescription,
                    Status = ticket.Status,
                    CreatedDate = ticket.CreatedDate,
                    AssignedToUserName = ticket.AssignedTo?.UserName,
                    AssignedToFullName = ticket.AssignedTo?.FullName,
                    Comments = ticket.Comments.Select(c => new CommentResponseDTO
                    {
                        CommentId = c.CommentId,
                        Content = c.Content,
                        CreatedDate = c.CreatedDate,
                        UserName = c.User?.FullName
                    }).ToList(),
                    Attachments = ticket.Attachments.Select(a => new AttachmentDTO
                    {
                        AttachmentId = a.AttachmentId,
                        FileName = a.FileName,
                        FileUrl = a.FileUrl
                    }).ToList()
                };

                return Ok(new APIResponse<TicketResponseDTO>(ticketDto) { Message = "Ticket retrieved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<TicketResponseDTO>(null, $"Internal server error: {ex.Message}"));
            }
        }

        // POST api/tickets
        [HttpPost]
        [CstsAuth(UserType.ExternalClient)]
        public async Task<ActionResult<APIResponse<TicketResponseDTO>>> Post([FromForm] CreateTicketDTO createDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = _unitOfWork.Users.GetById(Guid.Parse(userId));
                if (user == null || user.UserStatus != UserStatus.Active)
                    return BadRequest(new APIResponse<TicketResponseDTO>(null, "User is not active."));

                var ticket = new Ticket
                {
                    Product = createDto.Product,
                    ProblemDescription = createDto.ProblemDescription,
                    CreatedDate = DateTime.UtcNow,
                    Status = TicketStatus.New,
                    CreatedById = Guid.Parse(userId),
                    Attachments = new List<Attachment>()
                };

                var result = _validator.Validate(ticket);
                if (!result.IsValid)
                    return BadRequest(new APIResponse<TicketResponseDTO>(null, string.Join("; ", result.Errors.Select(e => e.ErrorMessage))));

                if (createDto.Attachments != null)
                {
                    foreach (var file in createDto.Attachments)
                    {
                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);
                        var filePath = _fileService.SaveFileAsync(memoryStream.ToArray(), FolderType.Images, Path.GetExtension(file.FileName));
                        ticket.Attachments.Add(new Attachment { FileName = file.FileName, FileUrl = filePath });
                    }
                }

                var response = _unitOfWork.Tickets.Add(ticket);
                if (!response)
                    return StatusCode(500, new APIResponse<TicketResponseDTO>(null, "Failed to create ticket"));

                var ticketDto = new TicketResponseDTO
                {
                    TicketId = ticket.TicketId,
                    Product = ticket.Product,
                    ProblemDescription = ticket.ProblemDescription,
                    Status = ticket.Status,
                    CreatedDate = ticket.CreatedDate,
                    AssignedToUserName = ticket.AssignedTo?.UserName,
                    AssignedToFullName = ticket.AssignedTo?.FullName,
                    Attachments = ticket.Attachments.Select(a => new AttachmentDTO { AttachmentId = a.AttachmentId, FileName = a.FileName, FileUrl = a.FileUrl }).ToList()
                };

                return CreatedAtAction(nameof(Get), new { id = ticket.TicketId }, new APIResponse<TicketResponseDTO>(ticketDto) { Message = "Ticket created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<TicketResponseDTO>(null, $"Internal server error: {ex.Message} - {ex.StackTrace}"));
            }
        }

        // PUT api/tickets/{id}
        [HttpPut("{id}")]
        [CstsAuth(UserType.SupportTeamMember)]
        public async Task<ActionResult<APIResponse<UpdateResponseDTO>>> Put(Guid id, [FromBody] UpdateTicketDTO updateDto)
        {
            try
            {
                var ticket = _unitOfWork.Tickets.GetById(id);
                if (ticket == null)
                    return NotFound(new APIResponse<UpdateResponseDTO>(null, "Ticket not found."));

                ticket.Status = updateDto.Status;

                var result = _validator.Validate(ticket);
                if (!result.IsValid)
                    return BadRequest(new APIResponse<UpdateResponseDTO>(null, string.Join("; ", result.Errors.Select(e => e.ErrorMessage))));

                var response = _unitOfWork.Tickets.Update(ticket);
                if (!response)
                    return StatusCode(500, new APIResponse<UpdateResponseDTO>(null, "Failed to update ticket status"));

                return Ok(new APIResponse<UpdateResponseDTO>(new UpdateResponseDTO { Success = true, Message = "Ticket status updated successfully" }) { Message = "Ticket status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<UpdateResponseDTO>(null, $"Internal server error: {ex.Message}"));
            }
        }

        // PUT api/tickets/Assign
        [HttpPut("Assign")]
        [CstsAuth(UserType.SupportManager)]
        public async Task<ActionResult<APIResponse<bool>>> AssignTicket([FromBody] AssignTicketDTO assignTicketDto)
        {
            try
            {
                var ticket = _unitOfWork.Tickets.GetById(assignTicketDto.TicketId);
                if (ticket == null)
                    return NotFound(new APIResponse<bool>(false, "Ticket not found."));

                ticket.AssignedToId = assignTicketDto.AssignedTo;
                ticket.Status = TicketStatus.Assigned;

                var response = _unitOfWork.Tickets.Update(ticket);
                if (response)
                    return Ok(new APIResponse<bool>(true) { Message = "Ticket assigned successfully." });

                return StatusCode(500, new APIResponse<bool>(false, "Failed to assign ticket."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<bool>(false, $"Internal server error: {ex.Message}"));
            }
        }

        // DELETE api/tickets/{id}
        [HttpDelete("{id}")]
        [CstsAuth(UserType.SupportManager)]
        public async Task<ActionResult<APIResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var response = _unitOfWork.Tickets.Delete(id);
                if (!response)
                    return NotFound(new APIResponse<bool>(false, "Ticket not found."));

                return Ok(new APIResponse<bool>(true) { Message = "Ticket deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<bool>(false, $"Internal server error: {ex.Message}"));
            }
        }

        // Health check endpoint
        [HttpGet("health")]
        public async Task<ActionResult<APIResponse<string>>> Health()
        {
            var canConnect = await _unitOfWork.CanConnectAsync();
            if (!canConnect)
                return StatusCode(500, new APIResponse<string>(null, "Database connection failed."));

            return Ok(new APIResponse<string>("Healthy") { Message = "Database connection successful." });
        }
    }
}
