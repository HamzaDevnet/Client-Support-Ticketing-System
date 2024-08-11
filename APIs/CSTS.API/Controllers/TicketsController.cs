using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSTS.DAL.Models;
using CSTS.DAL.Repository.IRepository;
using FluentValidation;
using CSTS.DAL.Enum;
using CSTS.DAL.DTOs;
using CSTS.DAL.Utilities;

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

        // GET: api/tickets
        [HttpGet("Summary")]
        public async Task<ActionResult<IEnumerable<TicketSummaryDTO>>> GetTicketSummary()
        {
            try
            {
                var tickets = await _unitOfWork.Tickets.GetAllIncludingAsync(t => t.AssignedTo);
                var ticketDtos = tickets.Data.Select(t => new TicketSummaryDTO
                {
                    TicketId = t.TicketId,
                    Product = t.Product,
                    Status = t.Status,
                    CreatedDate = t.CreatedDate,
                    AssignedToFullName = t.AssignedTo != null ? t.AssignedTo.FirstName : null
                }).ToList();

                return Ok(ticketDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // GET api/tickets/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketResponseDTO>> Get(Guid id)
        {
            try
            {
                var ticket = await _unitOfWork.Tickets.GetIncludingAsync(id, t => t.AssignedTo, t => t.Comments/*, t => t.Attachments*/);
                if (ticket.Data == null)
                    return NotFound("Ticket not found.");

                var ticketDto = new TicketResponseDTO
                {
                    TicketId = ticket.Data.TicketId,
                    Product = ticket.Data.Product,
                    ProblemDescription = ticket.Data.ProblemDescription,
                    Status = ticket.Data.Status,
                    CreatedDate = ticket.Data.CreatedDate,
                    AssignedToUserName = ticket.Data.AssignedTo?.UserName,
                    Comments = ticket.Data.Comments.Select(c => new CommentResponseDTO
                    {
                        CommentId = c.CommentId,
                        Content = c.Content,
                        CreatedDate = c.CreatedDate,
                        UserName = c.User?.UserName // Updated to use User property
                    }).ToList(),
                    //Attachments = ticket.Data.Attachments.Select(a => new AttachmentDTO
                    //{
                    //    AttachmentId = a.AttachmentId,
                    //    FileName = a.FileName,
                    //    FileUrl = a.FileUrl
                    //}).ToList()
                };

                return Ok(ticketDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/tickets
        [HttpPost]
        public async Task<ActionResult<TicketResponseDTO>> Post([FromForm] CreateTicketDTO createDto)
        {
            try
            {
                var ticket = new Ticket
                {
                    Product = createDto.Product,
                    ProblemDescription = createDto.ProblemDescription,
                    CreatedDate = DateTime.UtcNow,
                    Status = TicketStatus.New,
                    CreatedById = new Guid("ab1c2d34-5e6f-4b78-9c3d-1a2b3c4d5e6f"),
                    Attachments = new List<Attachment>() // Initialize Attachments
                };

                var result = _validator.Validate(ticket);
                if (!result.IsValid)
                    return BadRequest(result.Errors.Select(e => e.ErrorMessage));

                if (createDto.Attachments != null) // Check if Attachments are not null
                {
                    foreach (var file in createDto.Attachments)
                    {
                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);
                        var filePath = await _fileService.SaveFileAsync(memoryStream.ToArray(), FolderType.Images, Path.GetExtension(file.FileName));
                        ticket.Attachments.Add(new Attachment { FileName = file.FileName, FileUrl = filePath });
                    }
                }

                var response = await _unitOfWork.Tickets.AddAsync(ticket);
                if (!response.Data)
                    return StatusCode(500, "Failed to create ticket");

                var ticketDto = new TicketResponseDTO
                {
                    TicketId = ticket.TicketId,
                    Product = ticket.Product,
                    ProblemDescription = ticket.ProblemDescription,
                    Status = ticket.Status,
                    CreatedDate = ticket.CreatedDate,
                    AssignedToUserName = ticket.AssignedTo?.UserName,
                    Attachments = ticket.Attachments.Select(a => new AttachmentDTO { AttachmentId = a.AttachmentId, FileName = a.FileName, FileUrl = a.FileUrl }).ToList()
                };

                return CreatedAtAction(nameof(Get), new { id = ticket.TicketId }, ticketDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message} - {ex.StackTrace}");
            }
        }


        //PUT api/tickets/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateResponseDTO>> Put(Guid id, [FromBody] UpdateTicketDTO updateDto)
        {
            try
            {
                var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
                if (ticket.Data == null)
                    return NotFound("Ticket not found.");

                ticket.Data.Product = updateDto.Product;
                ticket.Data.ProblemDescription = updateDto.ProblemDescription;
                //ticket.Data.Attachments = updateDto.Attachments;
                ticket.Data.Status = updateDto.Status;
                ticket.Data.AssignedToId = updateDto.AssignedToId;

                var result = _validator.Validate(ticket.Data);
                if (!result.IsValid)
                    return BadRequest(new UpdateResponseDTO { Success = false, Message = string.Join("; ", result.Errors.Select(e => e.ErrorMessage)) });

                var response = await _unitOfWork.Tickets.UpdateAsync(ticket.Data);
                if (!response.Data)
                    return StatusCode(500, "Failed to update ticket");

                return Ok(new UpdateResponseDTO { Success = true, Message = "Ticket updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("Assign")]
        public async Task<ActionResult<WebResponse<bool>>> AssignTicket([FromBody] AssignTicketDTO assignTicketDto)
        {
            try
            {
                var ticket = await _unitOfWork.Tickets.GetByIdAsync(assignTicketDto.TicketId);
                if (ticket.Data == null)
                    return NotFound(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "Ticket not found." });

                ticket.Data.AssignedToId = assignTicketDto.AssignedTo;
                ticket.Data.Status = TicketStatus.Assigned;

                var response = await _unitOfWork.Tickets.UpdateAsync(ticket.Data);
                if (response.Data)
                    return Ok(new WebResponse<bool> { Data = true, Code = ResponseCode.Success, Message = "Ticket assigned successfully." });

                return StatusCode(500, new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = "Failed to assign ticket." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }


        // DELETE api/tickets/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            try
            {
                var response = await _unitOfWork.Tickets.DeleteAsync(id);
                if (!response.Data)
                    return NotFound("Ticket not found.");

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Health check endpoint
        [HttpGet("health")]
        public async Task<IActionResult> Health()
        {
            var canConnect = await _unitOfWork.CanConnectAsync();
            if (!canConnect)
                return StatusCode(500, "Database connection failed.");

            return Ok("Healthy");
        }
    }
}

