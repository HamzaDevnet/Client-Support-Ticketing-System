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
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(IUnitOfWork unitOfWork, IValidator<Ticket> validator, FileService fileService, ILogger<TicketsController> logger)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _fileService = fileService;
            _logger = logger;
        }

        [HttpGet("")]
        [CstsAuth(UserType.ExternalClient, UserType.SupportTeamMember, UserType.SupportManager)]
        public async Task<ActionResult<APIResponse<IEnumerable<TicketSummaryDTO>>>> GetTickets([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 100)
        {
            try
            {
                var userType = this.GetCurrentUserType();

                _logger.LogInformation("User type: {UserType}", userType);

                switch (userType)
                {
                    case UserType.ExternalClient:
                        return Ok(await GetClientTickets(PageNumber, PageSize));
                    case UserType.SupportTeamMember:
                        return Ok(await GetSupportTickets(PageNumber, PageSize));
                    case UserType.SupportManager:
                        return Ok(await GetManagerTickets(PageNumber, PageSize));
                    default:
                        _logger.LogWarning("Unknown user type: {UserType}", userType);
                        return Ok(new APIResponse<IEnumerable<TicketSummaryDTO>>(new List<TicketSummaryDTO>()));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting tickets");
                return Ok(new APIResponse<IEnumerable<TicketSummaryDTO>>(null, $"Internal server error: {ex.Message}"));
            }

            return Ok(new APIResponse<IEnumerable<TicketSummaryDTO>>(new List<TicketSummaryDTO>()));

        }

        private async Task<APIResponse<IEnumerable<TicketSummaryDTO>>> GetClientTickets([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 100)
        {
            _logger.LogInformation("Getting client tickets for user: {UserId}", this.GetCurrentUserId());
            try
            {
                var tickets = _unitOfWork.Tickets.Find(t => t.CreatedById == this.GetCurrentUserId(), PageNumber, PageSize, t => t.AssignedTo);

                var ticketDtos = tickets.Select(t => new TicketSummaryDTO
                {
                    TicketId = t.TicketId,
                    Product = t.Product,
                    Status = t.Status,
                    CreatedDate = t.CreatedDate,
                    AssignedToFullName = t.AssignedTo != null ? t.AssignedTo.FullName : "Not Assigned"
                }).ToList();
                _logger.LogInformation("Retrieved {Count} client tickets", ticketDtos.Count);
                return new APIResponse<IEnumerable<TicketSummaryDTO>>(ticketDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting client tickets");
                return new APIResponse<IEnumerable<TicketSummaryDTO>>(null, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<APIResponse<IEnumerable<TicketSummaryDTO>>> GetSupportTickets([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 100)
        {
            _logger.LogInformation("Getting Support tickets for user: {UserId}", this.GetCurrentUserId());
            try
            {
                var tickets = _unitOfWork.Tickets.Find(t => t.AssignedToId == this.GetCurrentUserId() && t.Status == TicketStatus.Assigned, PageNumber, PageSize, t => t.AssignedTo);
                var ticketDtos = tickets.Select(t => new TicketSummaryDTO
                {
                    TicketId = t.TicketId,
                    Product = t.Product,
                    Status = t.Status,
                    CreatedDate = t.CreatedDate,
                    AssignedToFullName = t.AssignedTo != null ? t.AssignedTo.FullName : "Not Assigned"
                }).ToList();
                _logger.LogInformation("Retrieved {Count} Support tickets", ticketDtos.Count);
                return new APIResponse<IEnumerable<TicketSummaryDTO>>(ticketDtos) { Message = "Assigned tickets retrieved successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting Support tickets");
                return new APIResponse<IEnumerable<TicketSummaryDTO>>(null, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<APIResponse<IEnumerable<TicketSummaryDTO>>> GetManagerTickets([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 100)
        {
            _logger.LogInformation("Getting Manager tickets for user: {UserId}", this.GetCurrentUserId());
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
                _logger.LogInformation("Retrieved {Count} Manager tickets", ticketDtos.Count);
                return new APIResponse<IEnumerable<TicketSummaryDTO>>(ticketDtos) { Message = "All tickets retrieved successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting Manager tickets");
                return new APIResponse<IEnumerable<TicketSummaryDTO>>(null, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/tickets/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<TicketResponseDTO>>> Get(Guid id)
        {
            _logger.LogError("LogError");
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");
            try
            {
                var ticket = _unitOfWork.Tickets.GetQueryable()
                    .Include(t => t.AssignedTo)
                    .Include(t => t.Comments).ThenInclude(c => c.User)
                    .Include(t => t.Attachments)
                    .FirstOrDefault(t => t.TicketId == id);

                if (ticket == null)
                    return Ok(new APIResponse<TicketResponseDTO>(null, "Ticket not found."));

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
                        UserId = c.UserId,
                        UserImage = c.User.Image,
                        CommentId = c.CommentId,
                        Content = c.Content,
                        CreatedDate = c.CreatedDate,
                        FullName = c.User?.FullName
                    }).ToList(),
                    Attachments = ticket.Attachments.Select(a => new AttachmentDto
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
                return Ok(new APIResponse<TicketResponseDTO>(null, $"Internal server error: {ex.Message}"));
            }
        }

        // POST api/tickets
        [HttpPost]
        [CstsAuth(UserType.ExternalClient)]
        public async Task<ActionResult<APIResponse<TicketResponseDTO>>> Post([FromBody] CreateTicketDTO createDto)
        {
            _logger.LogInformation("Creating new ticket for product: {Product}", createDto.Product);
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = _unitOfWork.Users.GetById(Guid.Parse(userId));
                if (user == null || user.UserStatus != UserStatus.Active)
                    return Ok(new APIResponse<TicketResponseDTO>(null, "User is not active."));

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
                    return Ok(new APIResponse<TicketResponseDTO>(null, string.Join("; ", result.Errors.Select(e => e.ErrorMessage))));

                if (createDto.Attachments != null)
                {
                    foreach (RequestAttachment file in createDto.Attachments)
                    {
                        //using var memoryStream = new MemoryStream();
                        // await file.CopyToAsync(memoryStream);
                        var filePath = _fileService.SaveFile(file, FolderType.Images);
                        ticket.Attachments.Add(new Attachment { FileName = " ", FileUrl = filePath });
                    }
                }

                var response = _unitOfWork.Tickets.Add(ticket);
                if (!response)
                    return Ok(new APIResponse<TicketResponseDTO>(null, "Failed to create ticket"));

                var ticketDto = new TicketResponseDTO
                {
                    TicketId = ticket.TicketId,
                    Product = ticket.Product,
                    ProblemDescription = ticket.ProblemDescription,
                    Status = ticket.Status,
                    CreatedDate = ticket.CreatedDate,
                    AssignedToUserName = ticket.AssignedTo?.UserName,
                    AssignedToFullName = ticket.AssignedTo?.FullName,
                    Attachments = ticket.Attachments.Select(a => new AttachmentDto { AttachmentId = a.AttachmentId, FileName = a.FileName, FileUrl = a.FileUrl }).ToList()
                };
                _logger.LogInformation("Ticket created successfully. TicketId: {TicketId}", ticket.TicketId);
                return CreatedAtAction(nameof(Get), new { id = ticket.TicketId }, new APIResponse<TicketResponseDTO>(ticketDto) { Message = "Ticket created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating ticket");
                return Ok(new APIResponse<TicketResponseDTO>(null, $"Internal server error: {ex.Message} - {ex.StackTrace}"));
            }
        }

        // PUT api/tickets/{id}
        [HttpPut("{id}")]
        [CstsAuth(UserType.SupportTeamMember)]
        public async Task<ActionResult<APIResponse<UpdateResponseDTO>>> Put([FromRoute]Guid id, [FromBody] UpdateTicketDTO updateDto)
        {
            _logger.LogError("LogError");
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");
            try
            {
                var ticket = _unitOfWork.Tickets.GetById(id);
                if (ticket == null)
                    return Ok(new APIResponse<UpdateResponseDTO>(null, "Ticket not found."));

                ticket.Status = updateDto.Status;

                var result = _validator.Validate(ticket);
                if (!result.IsValid)
                    return Ok(new APIResponse<UpdateResponseDTO>(null, string.Join("; ", result.Errors.Select(e => e.ErrorMessage))));

                var response = _unitOfWork.Tickets.Update(ticket);
                if (!response)
                    return Ok(new APIResponse<UpdateResponseDTO>(null, "Failed to update ticket status"));

                return Ok(new APIResponse<UpdateResponseDTO>(new UpdateResponseDTO { Success = true, Message = "Ticket status updated successfully" }) { Message = "Ticket status updated successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<UpdateResponseDTO>(null, $"Internal server error: {ex.Message}"));
            }
        }

        // PUT api/tickets/Assign
        [HttpPut("Close/{ticketId}")]
        [CstsAuth(UserType.SupportTeamMember)]
        public async Task<ActionResult<APIResponse<bool>>> CloseTicket([FromRoute] Guid ticketId)
        {
            _logger.LogError("LogError");
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");
            try
            {
                var ticket = _unitOfWork.Tickets.GetById(ticketId);
                if (ticket == null || ticket.AssignedToId != this.GetCurrentUserId())
                    return Ok(new APIResponse<bool>(false, "Ticket not found."));

                ticket.Status = TicketStatus.Closed;

                var response = _unitOfWork.Tickets.Update(ticket);
                if (response)
                    return Ok(new APIResponse<bool>(true) { Message = "Ticket closed successfully." });

                return Ok(new APIResponse<bool>(false, "Failed to close ticket."));
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<bool>(false, $"Internal server error: {ex.Message}"));
            }
        }
        
        // PUT api/tickets/Assign
        [HttpPut("Assign")]
        [CstsAuth(UserType.SupportManager)]
        public async Task<ActionResult<APIResponse<bool>>> AssignTicket([FromBody] AssignTicketDTO assignTicketDto)
        {
            _logger.LogError("LogError");
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");
            try
            {
                var ticket = _unitOfWork.Tickets.GetById(assignTicketDto.TicketId);
                if (ticket == null)
                    return Ok(new APIResponse<bool>(false, "Ticket not found."));

                ticket.AssignedToId = assignTicketDto.AssignedTo;
                ticket.Status = TicketStatus.Assigned;

                var response = _unitOfWork.Tickets.Update(ticket);
                if (response)
                    return Ok(new APIResponse<bool>(true) { Message = "Ticket assigned successfully." });

                return Ok(new APIResponse<bool>(false, "Failed to assign ticket."));
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<bool>(false, $"Internal server error: {ex.Message}"));
            }
        }

        // DELETE api/tickets/{id}
        [HttpPut("Remove/{id}")]
        [CstsAuth(UserType.SupportManager)]
        public async Task<ActionResult<APIResponse<bool>>> Delete(Guid id)
        {
            _logger.LogError("LogError");
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");
            try
            {
                var ticket = _unitOfWork.Tickets.GetById(id);
                if (ticket == null)
                    return Ok(new APIResponse<bool>(false, "Ticket not found."));
        
                ticket.AssignedToId = null;
                ticket.Status = TicketStatus.Removed;
                var response = _unitOfWork.Tickets.Update(ticket);

                return Ok(new APIResponse<bool>(true) { Message = "Ticket Marked As Removed." });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<bool>(false, $"Internal server error: {ex.Message}"));
            }
        }

        // Health check endpoint
        [HttpGet("health")]
        public async Task<ActionResult<APIResponse<string>>> Health()
        {
            _logger.LogError("LogError");
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");
            var canConnect = await _unitOfWork.CanConnectAsync();
            if (!canConnect)
                return Ok(new APIResponse<string>(null, "Database connection failed."));

            return Ok(new APIResponse<string>("Healthy") { Message = "Database connection successful." });
        }
    }
}
