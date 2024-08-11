using CSTS.DAL.Enum;
using CSTS.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CSTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 1. Count of tickets of each state
        [HttpGet("TicketsCountByState")]
        public IActionResult GetTicketsCountByState()
        {
            var tickets = _unitOfWork.Tickets.GetQueryable();
            if (!tickets.Any())
            {
                return NotFound("No tickets found.");
            }

            var result = tickets
                .GroupBy(t => t.Status)
                .Select(group => new
                {
                    State = group.Key.ToString(),
                    Count = group.Count()
                })
                .ToList();

            return Ok(result);
        }

        // 2. Each client with count of tickets he created
        [HttpGet("ClientTicketsCount")]
        public IActionResult GetClientTicketsCount()
        {
            var clients = _unitOfWork.Users.GetQueryable().Include(u => u.CreatedTickets).Where(u => u.UserType == UserType.ExternalClient);
            
            var result = clients
                .Select(client => new
                {
                    ClientName = client.FullName,
                    TicketsCount = client.CreatedTickets != null ? client.CreatedTickets.Count(): 0
                })
                .ToList();

            return Ok(result);
        }

        // 3. Each TeamMember with count of tickets assigned to him
        [HttpGet("TeamMemberTicketsCount")]
        public IActionResult GetTeamMemberTicketsCount()
        {
            var teamMembers = _unitOfWork.Users.GetQueryable().Where(u => u.UserType == UserType.SupportTeamMember).AsQueryable();
            if (!teamMembers.Any())
            {
                return NotFound("No team members found.");
            }

            var result = teamMembers
                .Select(member => new
                {
                    TeamMemberName = member.FullName,
                    TicketsAssignedCount = member.AssignedTickets != null ? member.AssignedTickets.Count() : 0
                })
                .ToList();

            return Ok(result);
        }

        // 4. Each TeamMember with count of tickets assigned to him and closed
        [HttpGet("TeamMemberClosedTicketsCount")]
        public IActionResult GetTeamMemberClosedTicketsCount()
        {
            var teamMembers = _unitOfWork.Users.GetQueryable().Where(u => u.UserType == UserType.SupportTeamMember).AsQueryable();
            if (!teamMembers.Any())
            {
                return NotFound("No team members found.");
            }

            var result = teamMembers
                .Select(member => new
                {
                    TeamMemberName = member.FullName,
                    TicketsClosedCount = member.AssignedTickets != null ? member.AssignedTickets.Count(t => t.Status == TicketStatus.Closed) : 0
                        
                })
                .ToList();

            return Ok(result);
        }
    }
}
