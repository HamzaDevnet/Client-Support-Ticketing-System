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

namespace CSTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Ticket> _validator;

        public TicketsController(IUnitOfWork unitOfWork, IValidator<Ticket> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        // GET: api/tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketSummaryDTO>>> Get()
        {
            try
            {
                var tickets = await _unitOfWork.Tickets.GetAllAsync();
                var ticketDtos = tickets.Data.Select(t => new TicketSummaryDTO
                {
                    TicketId = t.TicketId,
                    Product = t.Product,
                    Status = t.Status
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
                var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
                if (ticket.Data == null)
                    return NotFound("Ticket not found.");

                var ticketDto = new TicketResponseDTO
                {
                    TicketId = ticket.Data.TicketId,
                    Product = ticket.Data.Product,
                    ProblemDescription = ticket.Data.ProblemDescription,
                    Status = ticket.Data.Status,
                    CreatedDate = ticket.Data.CreatedDate,
                    AssignedToUserName = ticket.Data.AssignedTo?.UserName // Assuming User navigation property
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
        public async Task<ActionResult<TicketResponseDTO>> Post([FromBody] CreateTicketDTO createDto)
        {
            try
            {
                var ticket = new Ticket
                {
                    Product = createDto.Product,
                    ProblemDescription = createDto.ProblemDescription,
                    Attachments = createDto.Attachments,
                    AssignedToId = createDto.AssignedToId,
                    CreatedDate = DateTime.UtcNow,
                    Status = TicketStatus.New // Assuming new tickets are always 'New'
                };

                var result = _validator.Validate(ticket);
                if (!result.IsValid)
                    return BadRequest(result.Errors.Select(e => e.ErrorMessage));

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
                    AssignedToUserName = ticket.AssignedTo?.UserName
                };

                return CreatedAtAction(nameof(Get), new { id = ticket.TicketId }, ticketDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/tickets/{id}
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
                ticket.Data.Attachments = updateDto.Attachments;
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
    }
}
