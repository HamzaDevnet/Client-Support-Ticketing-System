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
        Deactivated = 0,
        Active = 1
    }
    public enum TicketStatus
    {
        New = 0,
        Assigned = 1,
        InProgress = 2,
        Closed = 3
    }

}
