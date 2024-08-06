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
        public async Task<ActionResult<WebResponse<IEnumerable<Ticket>>>> Get()
        {
            try
            {
                var response = await _unitOfWork.Tickets.GetAllAsync();
                return Ok(new WebResponse<IEnumerable<Ticket>>() { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<IEnumerable<Ticket>> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // GET api/tickets/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WebResponse<Ticket>>> Get(Guid id)
        {
            try
            {
                var response = await _unitOfWork.Tickets.GetByIdAsync(id);
                if (response.Data == null)
                {
                    return NotFound(new WebResponse<Ticket> { Data = null, Code = ResponseCode.Null, Message = "Ticket not found." });
                }
                return Ok(new WebResponse<Ticket> { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<Ticket> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // POST api/tickets
        [HttpPost]
        public async Task<ActionResult<WebResponse<Ticket>>> Post([FromBody] Ticket ticket)
        {
            try
            {
                if (ticket == null)
                {
                    return BadRequest(new WebResponse<Ticket> { Data = null, Code = ResponseCode.Null, Message = "Ticket cannot be null." });
                }

                // Validate ticket
                ValidationResult result = _validator.Validate(ticket);
                if (!result.IsValid)
                {
                    return BadRequest(new WebResponse<Ticket> { Data = null, Code = ResponseCode.Error, Message = result.Errors.ToString() });
                }

                var response = await _unitOfWork.Tickets.AddAsync(ticket);
                return CreatedAtAction(nameof(Get), new { id = ticket.TicketId }, new WebResponse<Ticket> { Data = ticket, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<Ticket> { Data = null, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // PUT api/tickets/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<WebResponse<bool>>> Put(Guid id, [FromBody] Ticket ticket)
        {
            try
            {
                if (ticket == null || ticket.TicketId != id)
                {
                    return BadRequest(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "Ticket is null or ID mismatch." });
                }

                var existingTicket = await _unitOfWork.Tickets.GetByIdAsync(id);
                if (existingTicket.Data == null)
                {
                    return NotFound(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "Ticket not found." });
                }

                // Validate ticket
                ValidationResult result = _validator.Validate(ticket);
                if (!result.IsValid)
                {
                    return BadRequest(new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = result.Errors.ToString() });
                }

                existingTicket.Data.Product = ticket.Product;
                existingTicket.Data.ProblemDescription = ticket.ProblemDescription;
                existingTicket.Data.CreatedDate = ticket.CreatedDate;
                existingTicket.Data.Status = ticket.Status;
                existingTicket.Data.CreatedById = ticket.CreatedById;
                existingTicket.Data.AssignedToId = ticket.AssignedToId;

                var response = await _unitOfWork.Tickets.UpdateAsync(existingTicket.Data);
                return Ok(new WebResponse<bool> { Data = response.Data, Code = ResponseCode.Success, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WebResponse<bool> { Data = false, Code = ResponseCode.Error, Message = ex.Message });
            }
        }

        // DELETE api/tickets/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<WebResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var response = await _unitOfWork.Tickets.DeleteAsync(id);
                if (!response.Data)
                {
                    return NotFound(new WebResponse<bool> { Data = false, Code = ResponseCode.Null, Message = "Ticket not found." });
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
