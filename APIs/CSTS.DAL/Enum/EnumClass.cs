using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.Enum
{
    public enum UserType
    {
        ExternalClient = 0,
        SupportTeamMember = 1,
        SupportManager = 2
    }

    public enum UserStatus
    {
        Active,
        Deactivated
    }
    public enum TicketStatus
    {
        New,
        Assigned,
        InProgress,
        Closed
    }

}
