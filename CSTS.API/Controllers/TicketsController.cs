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
        public async Task<ActionResult<IEnumerable<Ticket>>> Get()
        {
            var tickets = await _unitOfWork.Tickets.GetAllAsync();
            return Ok(tickets);
        }

        // GET api/tickets/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> Get(Guid id)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }

        // POST api/tickets
        [HttpPost]
        public async Task<ActionResult<Ticket>> Post([FromBody] Ticket ticket)
        {
            if (ticket == null)
            {
                return BadRequest("Ticket cannot be null.");
            }

            // Validate ticket
            ValidationResult result = _validator.Validate(ticket);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(Get), new { id = ticket.TicketId }, ticket);
        }

        // PUT api/tickets/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Ticket ticket)
        {
            if (ticket == null || ticket.TicketId != id)
            {
                return BadRequest("Ticket is null or ID mismatch.");
            }

            var existingTicket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (existingTicket == null)
            {
                return NotFound();
            }

            // Validate ticket
            ValidationResult result = _validator.Validate(ticket);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            existingTicket.Product = ticket.Product;
            existingTicket.ProblemDescription = ticket.ProblemDescription;
            existingTicket.CreatedDate = ticket.CreatedDate;
            existingTicket.Status = ticket.Status;
            existingTicket.CreatedById = ticket.CreatedById;
            existingTicket.AssignedToId = ticket.AssignedToId;

            await _unitOfWork.Tickets.UpdateAsync(existingTicket);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE api/tickets/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            await _unitOfWork.Tickets.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
