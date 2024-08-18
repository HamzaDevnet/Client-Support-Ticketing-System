using CSTS.API.ApiServices;
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
        private readonly ILogger<DashboardController> _logger;


        public DashboardController(IUnitOfWork unitOfWork , ILogger<DashboardController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // 1. Count of tickets of each state
        [HttpGet("TicketsCountByState")]
        [CstsAuth(UserType.SupportManager)]
        public IActionResult GetTicketsCountByState()
        {
            _logger.LogInformation("Fetching tickets count by state");

            var tickets = _unitOfWork.Tickets.GetQueryable();
            if (!tickets.Any())
            {
                _logger.LogWarning("No tickets found.");
                return Ok("No tickets found.");
            }

            var result = tickets
                .GroupBy(t => t.Status)
                .Select(group => new
                {
                    State = group.Key.ToString(),
                    Count = group.Count()
                })
                .ToList();
            _logger.LogInformation("Tickets count by state fetched successfully");
            return Ok(result);
        }

        // 2. Each client with count of tickets he created
        [HttpGet("ClientTicketsCount")]
        [CstsAuth(UserType.SupportManager)]
        public IActionResult GetClientTicketsCount()
        {
            _logger.LogInformation("Fetching count of tickets created by each client");
            var clients = _unitOfWork.Users.GetQueryable().Include(u => u.CreatedTickets).Where(u => u.UserType == UserType.ExternalClient);
            if (!clients.Any())
            {
                _logger.LogWarning("No clients found.");
                return Ok("No clients found.");
            }

            var result = clients
                .Select(client => new
                {
                    ClientName = client.FullName,
                    TicketsCount = client.CreatedTickets != null ? client.CreatedTickets.Count(): 0
                })
                .ToList();
            _logger.LogInformation("Client tickets count fetched successfully");
            return Ok(result);
        }

        // 3. Each TeamMember with count of tickets assigned to him
        [HttpGet("TeamMemberTicketsCount")]
        [CstsAuth(UserType.SupportManager)]
        public IActionResult GetTeamMemberTicketsCount()
        {
            _logger.LogInformation("Fetching count of tickets assigned to each team member");
            var teamMembers = _unitOfWork.Users.GetQueryable().Where(u => u.UserType == UserType.SupportTeamMember).AsQueryable();
            if (!teamMembers.Any())
            {
                _logger.LogWarning("No team members found.");
                return Ok("No team members found.");
            }

            var result = teamMembers
                .Select(member => new
                {
                    TeamMemberName = member.FullName,
                    TicketsAssignedCount = member.AssignedTickets != null ? member.AssignedTickets.Count() : 0
                })
                .ToList();
            _logger.LogInformation("Team member tickets count fetched successfully");
            return Ok(result);
        }

        // 4. Each TeamMember with count of tickets assigned to him and closed
        [HttpGet("TeamMemberClosedTicketsCount")]
        [CstsAuth(UserType.SupportManager)]
        public IActionResult GetTeamMemberClosedTicketsCount()
        {
            _logger.LogInformation("Fetching count of closed tickets assigned to each team member");

            var teamMembers = _unitOfWork.Users.GetQueryable().Where(u => u.UserType == UserType.SupportTeamMember).AsQueryable();
            if (!teamMembers.Any())
            {
                _logger.LogWarning("No team members found.");
                return Ok("No team members found.");
            }

            var result = teamMembers
                .Select(member => new
                {
                    TeamMemberName = member.FullName,
                    TicketsClosedCount = member.AssignedTickets != null ? member.AssignedTickets.Count(t => t.Status == TicketStatus.Closed) : 0
                        
                })
                .ToList();
            _logger.LogInformation("Team member closed tickets count fetched successfully");

            return Ok(result);
        }


    }
}
